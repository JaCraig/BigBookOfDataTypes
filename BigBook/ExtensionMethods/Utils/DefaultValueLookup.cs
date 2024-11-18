using System;
using System.Collections.Generic;

namespace BigBook.ExtensionMethods.Utils
{
    /// <summary>
    /// Default value lookup
    /// </summary>
    public static class DefaultValueLookup
    {
        /// <summary>
        /// The values
        /// </summary>
        public static Dictionary<int, object?> Values = new Dictionary<int, object?>
        {
            [typeof(byte).GetHashCode()] = default(byte),
            [typeof(sbyte).GetHashCode()] = default(sbyte),
            [typeof(short).GetHashCode()] = default(short),
            [typeof(int).GetHashCode()] = default(int),
            [typeof(long).GetHashCode()] = default(long),
            [typeof(ushort).GetHashCode()] = default(ushort),
            [typeof(uint).GetHashCode()] = default(uint),
            [typeof(ulong).GetHashCode()] = default(ulong),
            [typeof(double).GetHashCode()] = default(double),
            [typeof(float).GetHashCode()] = default(float),
            [typeof(decimal).GetHashCode()] = default(decimal),
            [typeof(bool).GetHashCode()] = default(bool),
            [typeof(char).GetHashCode()] = default(char),

            [typeof(byte?).GetHashCode()] = default(byte?),
            [typeof(sbyte?).GetHashCode()] = default(sbyte?),
            [typeof(short?).GetHashCode()] = default(short?),
            [typeof(int?).GetHashCode()] = default(int?),
            [typeof(long?).GetHashCode()] = default(long?),
            [typeof(ushort?).GetHashCode()] = default(ushort?),
            [typeof(uint?).GetHashCode()] = default(uint?),
            [typeof(ulong?).GetHashCode()] = default(ulong?),
            [typeof(double?).GetHashCode()] = default(double?),
            [typeof(float?).GetHashCode()] = default(float?),
            [typeof(decimal?).GetHashCode()] = default(decimal?),
            [typeof(bool?).GetHashCode()] = default(bool?),
            [typeof(char?).GetHashCode()] = default(char?),

            [typeof(string).GetHashCode()] = default(string),
            [typeof(Guid).GetHashCode()] = default(Guid),
            [typeof(DateTime).GetHashCode()] = default(DateTime),
            [typeof(DateTimeOffset).GetHashCode()] = default(DateTimeOffset),
            [typeof(TimeSpan).GetHashCode()] = default(TimeSpan),
            [typeof(Guid?).GetHashCode()] = default(Guid?),
            [typeof(DateTime?).GetHashCode()] = default(DateTime?),
            [typeof(DateTimeOffset?).GetHashCode()] = default(DateTimeOffset?),
            [typeof(TimeSpan?).GetHashCode()] = default(TimeSpan?),
            [typeof(byte[]).GetHashCode()] = default(byte[]),
        };
    }
}