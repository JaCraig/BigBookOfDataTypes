using BenchmarkDotNet.Attributes;
using BigBook.Conversion;
using BigBook.ExtensionMethods.Utils;
using BigBook.Registration;
using Fast.Activator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace BigBook.Benchmarks.Tests
{
    public static class TestExtensions
    {
        /// <summary>
        /// The converters
        /// </summary>
        private static readonly Dictionary<Type, TypeConverter> Converters = new Dictionary<Type, TypeConverter>
        {
            [typeof(SqlDbType)] = new SqlDbTypeTypeConverter(),
            [typeof(DbType)] = new DbTypeTypeConverter()
        };

        /// <summary>
        /// Attempts to convert the object to another type and returns the value
        /// </summary>
        /// <typeparam name="TObject">Type to convert from</typeparam>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="item">Object to convert</param>
        /// <param name="defaultValue">
        /// Default value to return if there is an issue or it can't be converted
        /// </param>
        /// <returns>
        /// The object converted to the other type or the default value if there is an error or
        /// can't be converted
        /// </returns>
        public static TReturn NewTo<TObject, TReturn>(this TObject item, TReturn defaultValue = default)
        {
            if (item is TReturn ReturnValue)
                return ReturnValue;
            return (TReturn)item.NewTo(typeof(TReturn), defaultValue)!;
        }

        /// <summary>
        /// Attempts to convert the object to another type and returns the value
        /// </summary>
        /// <typeparam name="TObject">Type to convert from</typeparam>
        /// <param name="item">Object to convert</param>
        /// <param name="resultType">Result type</param>
        /// <param name="defaultValue">
        /// Default value to return if there is an issue or it can't be converted
        /// </param>
        /// <returns>
        /// The object converted to the other type or the default value if there is an error or
        /// can't be converted
        /// </returns>
        public static object? NewTo<TObject>(this TObject item, Type resultType, object? defaultValue = null)
        {
            if (resultType is null)
                return item;
            try
            {
                if (item is null || item is DBNull)
                {
                    return ReturnDefaultValue(resultType, defaultValue);
                }
                var ObjectType = item.GetType();
                if (resultType.IsAssignableFrom(ObjectType))
                {
                    return item;
                }

                if (!Converters.TryGetValue(ObjectType, out var Converter))
                    Converter = TypeDescriptor.GetConverter(ObjectType);
                if (Converter.CanConvertTo(resultType))
                {
                    return Converter.ConvertTo(item, resultType);
                }

                if (!Converters.TryGetValue(resultType, out Converter))
                    Converter = TypeDescriptor.GetConverter(resultType);
                if (Converter.CanConvertFrom(ObjectType))
                {
                    return Converter.ConvertFrom(item);
                }

                if (resultType.IsEnum)
                {
                    if (item is string ItemStringValue)
                    {
                        return Enum.Parse(resultType, ItemStringValue, true);
                    }

                    return Enum.ToObject(resultType, item);
                }

                var IEnumerableResultType = resultType.GetIEnumerableElementType();
                var IEnumerableObjectType = ObjectType.GetIEnumerableElementType();
                if (resultType != IEnumerableResultType && ObjectType != IEnumerableObjectType)
                {
                    var TempList = (IList)FastActivator.CreateInstance(typeof(List<>).MakeGenericType(IEnumerableResultType));
                    foreach (var Item in (IEnumerable)item)
                    {
                        TempList.Add(Item.To(IEnumerableResultType, null));
                    }
                    return TempList;
                }
                if (resultType.IsClass)
                {
                    var ReturnValue = FastActivator.CreateInstance(resultType);
                    ObjectType.MapTo(resultType)
                                ?.AutoMap()
                                .Copy(item, ReturnValue);
                    return ReturnValue;
                }

                try
                {
                    return Convert.ChangeType(item, resultType, CultureInfo.InvariantCulture);
                }
                catch { }
            }
            catch
            {
            }
            return ReturnDefaultValue(resultType, defaultValue);

            static object? ReturnDefaultValue(Type resultType, object? defaultValue)
            {
                if (!(defaultValue is null) || !resultType.IsValueType)
                    return defaultValue;
                var ResultHash = resultType.GetHashCode();
                if (DefaultValueLookup.Values.TryGetValue(ResultHash, out var ReturnValue))
                    return ReturnValue;
                return FastActivator.CreateInstance(resultType);
            }
        }
    }

    [RankColumn, MemoryDiagnoser]
    public class ToRedux
    {
        [Params(100)]
        public int Count;

        [Benchmark(Baseline = true)]
        public void BaseTest()
        {
            for (var x = 0; x < Count; ++x)
            {
                _ = "http://A".To(new Uri("http://B"));
                _ = new Uri("http://A").To("http://B");
                var Value = DBNull.Value;
                _ = Value.To(typeof(string), "A");
                _ = Value.To(typeof(DateTime), null);
                _ = 1.To(typeof(TestEnum), null);
                _ = "Value3".To(typeof(TestEnum), null);
            }
        }

        [Benchmark]
        public void NewTest()
        {
            for (var x = 0; x < Count; ++x)
            {
                _ = "http://A".NewTo(new Uri("http://B"));
                _ = new Uri("http://A").NewTo("http://B");
                var Value = DBNull.Value;
                _ = Value.NewTo(typeof(string), "A");
                _ = Value.NewTo(typeof(DateTime), null);
                _ = 1.NewTo(typeof(TestEnum), null);
                _ = "Value3".NewTo(typeof(TestEnum), null);
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            Canister.Builder.CreateContainer(null).RegisterBigBookOfDataTypes().AddAssembly(typeof(DynamoTests).Assembly).Build();
        }

        private enum TestEnum
        {
            Value1 = 0,
            Value2,
            Value3
        }

        private class TestClass
        {
            public string A { get; set; }
        }
    }
}