/*
Copyright 2016 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using BigBook.Properties;
using BigBook.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BigBook
{
    /// <summary>
    /// Version info
    /// </summary>
    [Flags]
    public enum VersionInfo
    {
        /// <summary>
        /// Short version
        /// </summary>
        ShortVersion = 0,

        /// <summary>
        /// Long version
        /// </summary>
        LongVersion = 1
    }

    /// <summary>
    /// Reflection oriented extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the attribute from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="provider">Attribute provider</param>
        /// <param name="inherit">
        /// When true, it looks up the heirarchy chain for the inherited custom attributes
        /// </param>
        /// <returns>Attribute specified if it exists</returns>
        [return: MaybeNull]
        public static T Attribute<T>(this MemberInfo provider, bool inherit = true) where T : Attribute
        {
            var TempAttributes = provider.Attributes<T>(inherit);
            return TempAttributes.Length > 0 ? TempAttributes[0] : default!;
        }

        /// <summary>
        /// Gets the attributes from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="provider">Attribute provider</param>
        /// <param name="inherit">
        /// When true, it looks up the heirarchy chain for the inherited custom attributes
        /// </param>
        /// <returns>Array of attributes</returns>
        public static T[] Attributes<T>(this MemberInfo provider, bool inherit = true) where T : Attribute => provider?.GetCustomAttributes(typeof(T), inherit).ToArray(x => (T)x) ?? Array.Empty<T>();

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <typeparam name="TReturnType">Return type expected</typeparam>
        /// <param name="inputObject">Object to call the method on</param>
        /// <param name="methodName">Method name</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <returns>The returned value of the method</returns>
        /// <exception cref="ArgumentNullException">inputObject or methodName</exception>
        /// <exception cref="InvalidOperationException">
        /// Could not find method " + methodName + " with the appropriate input variables.
        /// </exception>
        public static TReturnType Call<TReturnType>(this object inputObject, string methodName, params object[] inputVariables)
        {
            if (inputObject is null)
            {
                throw new ArgumentNullException(nameof(inputObject));
            }

            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            if (inputVariables is null)
            {
                inputVariables = Array.Empty<object>();
            }

            var ObjectType = inputObject.GetType();
            var MethodInputTypes = new Type[inputVariables.Length];
            for (var x = 0; x < inputVariables.Length; ++x)
            {
                MethodInputTypes[x] = inputVariables[x].GetType();
            }

            var Method = ObjectType.GetMethod(methodName, MethodInputTypes);
            if (Method is null)
            {
                throw new InvalidOperationException("Could not find method " + methodName + " with the appropriate input variables.");
            }

            return (TReturnType)Method.Invoke(inputObject, inputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <typeparam name="TGenericType1">Generic method type 1</typeparam>
        /// <typeparam name="TReturnType">Return type expected</typeparam>
        /// <param name="inputObject">Object to call the method on</param>
        /// <param name="methodName">Method name</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <returns>The returned value of the method</returns>
        /// <exception cref="ArgumentNullException">inputObject or methodName</exception>
        /// <exception cref="InvalidOperationException">
        /// Could not find method " + methodName + " with the appropriate input variables.
        /// </exception>
        public static TReturnType Call<TGenericType1, TReturnType>(this object inputObject, string methodName, params object[] inputVariables)
        {
            if (inputObject is null)
            {
                throw new ArgumentNullException(nameof(inputObject));
            }

            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            if (inputVariables is null)
            {
                inputVariables = Array.Empty<object>();
            }

            var ObjectType = inputObject.GetType();
            var MethodInputTypes = new Type[inputVariables.Length];
            for (var x = 0; x < inputVariables.Length; ++x)
            {
                MethodInputTypes[x] = inputVariables[x].GetType();
            }

            var Method = ObjectType.GetMethod(methodName, MethodInputTypes);
            if (Method is null)
            {
                throw new InvalidOperationException("Could not find method " + methodName + " with the appropriate input variables.");
            }

            Method = Method.MakeGenericMethod(typeof(TGenericType1));
            return inputObject.Call<TReturnType>(Method, inputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <typeparam name="TGenericType1">Generic method type 1</typeparam>
        /// <typeparam name="TGenericType2">Generic method type 2</typeparam>
        /// <typeparam name="TReturnType">Return type expected</typeparam>
        /// <param name="inputObject">Object to call the method on</param>
        /// <param name="methodName">Method name</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <returns>The returned value of the method</returns>
        /// <exception cref="ArgumentNullException">inputObject or methodName</exception>
        /// <exception cref="InvalidOperationException">
        /// Could not find method " + methodName + " with the appropriate input variables.
        /// </exception>
        public static TReturnType Call<TGenericType1, TGenericType2, TReturnType>(this object inputObject, string methodName, params object[] inputVariables)
        {
            if (inputObject is null)
            {
                throw new ArgumentNullException(nameof(inputObject));
            }

            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            if (inputVariables is null)
            {
                inputVariables = Array.Empty<object>();
            }

            var ObjectType = inputObject.GetType();
            var MethodInputTypes = new Type[inputVariables.Length];
            for (var x = 0; x < inputVariables.Length; ++x)
            {
                MethodInputTypes[x] = inputVariables[x].GetType();
            }

            var Method = ObjectType.GetMethod(methodName, MethodInputTypes);
            if (Method is null)
            {
                throw new InvalidOperationException("Could not find method " + methodName + " with the appropriate input variables.");
            }

            Method = Method.MakeGenericMethod(typeof(TGenericType1), typeof(TGenericType2));
            return inputObject.Call<TReturnType>(Method, inputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <typeparam name="TGenericType1">Generic method type 1</typeparam>
        /// <typeparam name="TGenericType2">Generic method type 2</typeparam>
        /// <typeparam name="TGenericType3">Generic method type 3</typeparam>
        /// <typeparam name="TReturnType">Return type expected</typeparam>
        /// <param name="inputObject">Object to call the method on</param>
        /// <param name="methodName">Method name</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <returns>The returned value of the method</returns>
        /// <exception cref="ArgumentNullException">inputObject or methodName</exception>
        /// <exception cref="InvalidOperationException">
        /// Could not find method " + methodName + " with the appropriate input variables.
        /// </exception>
        public static TReturnType Call<TGenericType1, TGenericType2, TGenericType3, TReturnType>(this object inputObject, string methodName, params object[] inputVariables)
        {
            if (inputObject is null)
            {
                throw new ArgumentNullException(nameof(inputObject));
            }

            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            if (inputVariables is null)
            {
                inputVariables = Array.Empty<object>();
            }

            var ObjectType = inputObject.GetType();
            var MethodInputTypes = new Type[inputVariables.Length];
            for (var x = 0; x < inputVariables.Length; ++x)
            {
                MethodInputTypes[x] = inputVariables[x].GetType();
            }

            var Method = ObjectType.GetMethod(methodName, MethodInputTypes);
            if (Method is null)
            {
                throw new InvalidOperationException("Could not find method " + methodName + " with the appropriate input variables.");
            }

            Method = Method.MakeGenericMethod(typeof(TGenericType1), typeof(TGenericType2), typeof(TGenericType3));
            return inputObject.Call<TReturnType>(Method, inputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <typeparam name="TReturnType">Return type expected</typeparam>
        /// <param name="inputObject">Object to call the method on</param>
        /// <param name="method">Method</param>
        /// <param name="inputVariables">(Optional)input variables for the method</param>
        /// <returns>The returned value of the method</returns>
        /// <exception cref="ArgumentNullException">inputObject or method</exception>
        public static TReturnType Call<TReturnType>(this object inputObject, MethodInfo method, params object[] inputVariables)
        {
            if (inputObject is null)
            {
                throw new ArgumentNullException(nameof(inputObject));
            }

            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (inputVariables is null)
            {
                inputVariables = Array.Empty<object>();
            }

            return (TReturnType)method.Invoke(inputObject, inputVariables);
        }

        /// <summary>
        /// Creates an instance of the type and casts it to the specified type
        /// </summary>
        /// <typeparam name="TClassType">Class type to return</typeparam>
        /// <param name="type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static TClassType Create<TClassType>(this Type type, params object[] args) => type is null ? default : (TClassType)type?.Create(args)!;

        /// <summary>
        /// Creates an instance of the type
        /// </summary>
        /// <param name="type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static object? Create(this Type type, params object[] args) => type is null ? null : Activator.CreateInstance(type, args);

        /// <summary>
        /// Creates an instance of the types and casts it to the specified type
        /// </summary>
        /// <typeparam name="TClassType">Class type to return</typeparam>
        /// <param name="types">Types to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the types</returns>
        public static IEnumerable<TClassType> Create<TClassType>(this IEnumerable<Type> types, params object[] args)
        {
            if (types?.Any() != true)
            {
                yield break;
            }

            foreach (var Type in types)
            {
                yield return Type.Create<TClassType>(args);
            }
        }

        /// <summary>
        /// Creates an instance of the types specified
        /// </summary>
        /// <param name="types">Types to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the types</returns>
        public static IEnumerable<object?> Create(this IEnumerable<Type> types, params object[] args)
        {
            if (types?.Any() != true)
            {
                yield break;
            }

            foreach (var Type in types)
            {
                yield return Type.Create(args);
            }
        }

        /// <summary>
        /// Gets the type of the element within the IEnumerable. Or the type itself if it is not an IEnumerable.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>The element type if it is an IEnumerable, otherwise the type sent in.</returns>
        public static Type GetIEnumerableElementType(this Type type)
        {
            var IEnum = FindIEnumerableElementType(type);

            return IEnum is null ? type : IEnum.GetGenericArguments()[0];
        }

        /// <summary>
        /// Gets the method specified based on name and parameter types
        /// </summary>
        /// <param name="ObjectType">Type of the object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="MethodInputTypes">The method input types.</param>
        /// <returns>The method found or null if it is not available</returns>
        public static MethodInfo? GetMethod(this Type ObjectType, string methodName, Type[] MethodInputTypes)
        {
            if (ObjectType is null)
                return null;
            return Array.Find(ObjectType.GetMethods(), x =>
                            {
                                if (x.Name != methodName)
                                {
                                    return false;
                                }

                                var TempParameters = x.GetParameters();
                                if (TempParameters.Length != MethodInputTypes.Length)
                                {
                                    return false;
                                }

                                for (var y = 0; y < MethodInputTypes.Length; ++y)
                                {
                                    if (MethodInputTypes[y] != TempParameters[y].ParameterType)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            });
        }

        /// <summary>
        /// Returns the type's name (Actual C# name, not the funky version from the Name property)
        /// </summary>
        /// <param name="objectType">Type to get the name of</param>
        /// <returns>string name of the type</returns>
        public static string GetName(this Type objectType)
        {
            if (objectType is null)
            {
                return "";
            }

            var Output = new StringBuilder();
            if (objectType.Name == "Void")
            {
                Output.Append("void");
            }
            else
            {
                Output.Append(objectType.DeclaringType is null ? objectType.Namespace : objectType.DeclaringType.GetName())
                    .Append(".");
                if (objectType.Name.Contains("`", StringComparison.Ordinal))
                {
                    var GenericTypes = objectType.GetGenericArguments();
                    Output
                        .Append(objectType.Name, 0, objectType.Name.IndexOf("`", StringComparison.OrdinalIgnoreCase))
                        .Append("<");
                    var Seperator = "";
                    for (int x = 0, GenericTypesLength = GenericTypes.Length; x < GenericTypesLength; x++)
                    {
                        var GenericType = GenericTypes[x];
                        Output.Append(Seperator).Append(GenericType.GetName());
                        Seperator = ",";
                    }

                    Output.Append(">");
                }
                else
                {
                    Output.Append(objectType.Name);
                }
            }
            return Output.ToString().Replace("&", "", StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the property recursively.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="recursively">if set to <c>true</c> [recursively].</param>
        /// <returns>The property</returns>
        public static PropertyInfo? GetProperty(this Type? type, string name, bool recursively)
        {
            if (type is null || string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (recursively)
            {
                var Result = type.GetProperty(name, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (!(Result is null) || !type.IsInterface)
                    return Result;
                var Interfaces = type.GetInterfaces();
                for (var x = 0; x < Interfaces.Length; ++x)
                {
                    Result = Interfaces[x].GetProperty(name);
                    if (!(Result is null))
                        return Result;
                }
            }
            return type.GetProperty(name, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static PropertyInfo? GetProperty<TObject>(this Type type, string name)
        {
            if (type is null)
                return null;
            PropertyInfo? Result = Array.Find(TypeCacheFor<TObject>.Properties, x => x.Name == name);
            if (!(Result is null) || !type.IsInterface)
                return Result;
            var Interfaces = TypeCacheFor<TObject>.Interfaces;
            for (var x = 0; x < Interfaces.Length; ++x)
            {
                Result = Interfaces[x].GetProperty(name, false);
                if (!(Result is null))
                    return Result;
            }
            return null;
        }

        /// <summary>
        /// Determines if the type has a default constructor
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if it does, false otherwise</returns>
        public static bool HasDefaultConstructor(this Type type)
        {
            return type?
                .GetConstructors()
                .Any(x => x.GetParameters().Length == 0) == true;
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="inputObject">Object</param>
        /// <param name="type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this object inputObject, Type type) => inputObject?.GetType().Is(type) == true;

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this Type objectType, Type type) => type?.IsAssignableFrom(objectType) == true;

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="inputObject">Object</param>
        /// <typeparam name="TBaseObjectType">Base object type</typeparam>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is<TBaseObjectType>(this object inputObject) => inputObject is TBaseObjectType;

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <typeparam name="TBaseObjectType">Base object type</typeparam>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is<TBaseObjectType>(this Type objectType) => objectType?.Is(typeof(TBaseObjectType)) == true;

        /// <summary>
        /// Determines whether this instance is debug.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns><c>true</c> if the specified assembly is debug; otherwise, <c>false</c>.</returns>
        public static bool IsDebug(this Assembly assembly) => assembly.GetCustomAttributes().OfType<DebuggableAttribute>().SingleOrDefault()?.IsJitTrackingEnabled() ?? false;

        /// <summary>
        /// Determines whether [is JIT optimized].
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns><c>true</c> if [is JIT optimized] [the specified assembly]; otherwise, <c>false</c>.</returns>
        public static bool IsJitOptimized(this Assembly assembly) => assembly.GetCustomAttributes().OfType<DebuggableAttribute>().SingleOrDefault()?.IsJitOptimized() ?? false;

        /// <summary>
        /// Makes a shallow copy of the object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="inputObject">Object to copy</param>
        /// <param name="simpleTypesOnly">
        /// If true, it only copies simple types (no classes, only items like int, string, etc.),
        /// false copies everything.
        /// </param>
        /// <returns>A copy of the object</returns>
        public static T MakeShallowCopy<T>(this T inputObject, bool simpleTypesOnly = false)
        {
            if (Equals(inputObject, default(T)!))
            {
                return default!;
            }

            var ObjectType = inputObject?.GetType();
            if (ObjectType is null)
                return default!;
            var ClassInstance = ObjectType.Create<T>();
            var TempProperties = ObjectType.GetProperties();
            for (int x = 0, maxLength = TempProperties.Length; x < maxLength; x++)
            {
                var TempProperty = TempProperties[x];
                if (TempProperty.CanRead
                        && TempProperty.CanWrite
                        && simpleTypesOnly
                        && TempProperty.PropertyType.IsValueType)
                {
                    TempProperty.SetValue(ClassInstance, TempProperty.GetValue(inputObject, null), null);
                }
                else if (!simpleTypesOnly
                           && TempProperty.CanRead
                           && TempProperty.CanWrite)
                {
                    TempProperty.SetValue(ClassInstance, TempProperty.GetValue(inputObject, null), null);
                }
            }
            var TempFields = ObjectType.GetFields();
            for (int x = 0, TempFieldsLength = TempFields.Length; x < TempFieldsLength; x++)
            {
                var Field = TempFields[x];
                if (simpleTypesOnly && Field.IsPublic)
                {
                    Field.SetValue(ClassInstance, Field.GetValue(inputObject));
                }
                else if (!simpleTypesOnly && Field.IsPublic)
                {
                    Field.SetValue(ClassInstance, Field.GetValue(inputObject));
                }
            }

            return ClassInstance;
        }

        /// <summary>
        /// Goes through a list of types and determines if they're marked with a specific attribute
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="types">Types to check</param>
        /// <param name="inherit">
        /// When true, it looks up the heirarchy chain for the inherited custom attributes
        /// </param>
        /// <returns>The list of types that are marked with an attribute</returns>
        public static IEnumerable<Type> MarkedWith<T>(this IEnumerable<Type> types, bool inherit = true)
            where T : Attribute => types?.Where(x => x.IsDefined(typeof(T), inherit) && !x.IsAbstract) ?? Array.Empty<Type>();

        /// <summary>
        /// Returns an instance of all classes that it finds within an assembly that are of the
        /// specified base type/interface.
        /// </summary>
        /// <typeparam name="TClassType">Base type/interface searching for</typeparam>
        /// <param name="assembly">Assembly to search within</param>
        /// <param name="args">Args used to create the object</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<TClassType> Objects<TClassType>(this Assembly assembly, params object[] args)
        {
            return assembly?
                .Types<TClassType>()
                .Where(x => !x.ContainsGenericParameters)
                .Create<TClassType>(args) ?? Array.Empty<TClassType>();
        }

        /// <summary>
        /// Returns an instance of all classes that it finds within a group of assemblies that are
        /// of the specified base type/interface.
        /// </summary>
        /// <typeparam name="TClassType">Base type/interface searching for</typeparam>
        /// <param name="assemblies">Assemblies to search within</param>
        /// <param name="args">Args used to create the object</param>
        /// <returns>A list of objects that are of the type specified</returns>
        public static IEnumerable<TClassType> Objects<TClassType>(this IEnumerable<Assembly> assemblies, params object[] args)
        {
            if (assemblies?.Any() != true)
            {
                yield break;
            }

            foreach (var Assembly in assemblies)
            {
                foreach (var Object in Assembly.Objects<TClassType>(args))
                {
                    yield return Object;
                }
            }
        }

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="inputObject">The object to get the property of</param>
        /// <param name="property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object? Property(this object inputObject, PropertyInfo property) => inputObject is null || property is null ? null : property.GetValue(inputObject, null);

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="inputObject">The object to get the property of</param>
        /// <param name="property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        public static object? Property(this object inputObject, string property)
        {
            if (inputObject is null || string.IsNullOrEmpty(property))
            {
                return null;
            }

            var Properties = property.Split(new string[] { "." }, StringSplitOptions.None);
            object? TempObject = inputObject;
            Type? TempObjectType = TempObject.GetType();
            PropertyInfo? DestinationProperty;
            for (var x = 0; x < Properties.Length - 1; ++x)
            {
                DestinationProperty = TempObjectType.GetProperty(Properties[x], true);
                TempObjectType = DestinationProperty?.PropertyType;
                TempObject = DestinationProperty?.GetValue(TempObject, null);
                if (TempObject is null)
                {
                    return null;
                }
            }
            DestinationProperty = TempObjectType.GetProperty(Properties[^1], true);
            return DestinationProperty is null ? null : TempObject.Property(DestinationProperty);
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="inputObject">The object to set the property of</param>
        /// <param name="property">The property to set</param>
        /// <param name="value">Value to set the property to</param>
        /// <param name="format">Allows for formatting if the destination is a string</param>
        public static object? Property(this object inputObject, PropertyInfo property, object value, string format = "")
        {
            if (inputObject is null)
            {
                return null;
            }

            if (property is null || value is null)
            {
                return inputObject;
            }

            if (property.PropertyType == typeof(string))
            {
                value = value.FormatToString(format);
            }

            property.SetValue(inputObject, value.To(property.PropertyType, null), null);
            return inputObject;
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="inputObject">The object to set the property of</param>
        /// <param name="property">The property to set</param>
        /// <param name="value">Value to set the property to</param>
        /// <param name="format">Allows for formatting if the destination is a string</param>
        public static object? Property(this object inputObject, string property, object value, string format = "")
        {
            if (inputObject is null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(property) || value is null)
            {
                return inputObject;
            }

            var Properties = property.Split(new string[] { "." }, StringSplitOptions.None);
            object? TempObject = inputObject;
            Type? TempObjectType = TempObject.GetType();
            PropertyInfo? DestinationProperty;
            for (var x = 0; x < Properties.Length - 1; ++x)
            {
                DestinationProperty = TempObjectType.GetProperty(Properties[x], true);
                TempObjectType = DestinationProperty?.PropertyType;
                TempObject = DestinationProperty?.GetValue(TempObject, null);
                if (TempObject is null)
                {
                    return inputObject;
                }
            }
            DestinationProperty = TempObjectType.GetProperty(Properties[^1], true);
            if (DestinationProperty is null)
            {
                throw new NullReferenceException(Resources.PropertyInfoCantBeNull);
            }

            TempObject.Property(DestinationProperty, value, format);
            return inputObject;
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="TClassType">Class type</typeparam>
        /// <typeparam name="TDataType">Data type expecting</typeparam>
        /// <param name="property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        public static Expression<Func<TClassType, TDataType>> PropertyGetter<TClassType, TDataType>(this PropertyInfo property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!property.PropertyType.Is(typeof(TDataType)))
            {
                throw new ArgumentException(Resources.PropertyOfWrongType);
            }

            if (!property.DeclaringType.Is(typeof(TClassType)) && !typeof(TClassType).Is(property.DeclaringType))
            {
                throw new ArgumentException(Resources.PropertyNotOfDeclaringType);
            }

            var ObjectInstance = Expression.Parameter(property.DeclaringType, "x");
            var PropertyGet = Expression.Property(ObjectInstance, property);
            if (property.PropertyType != typeof(TDataType))
            {
                var Convert = Expression.Convert(PropertyGet, typeof(TDataType));
                return Expression.Lambda<Func<TClassType, TDataType>>(Convert, ObjectInstance);
            }
            return Expression.Lambda<Func<TClassType, TDataType>>(PropertyGet, ObjectInstance);
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="TClassType">Class type</typeparam>
        /// <param name="property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        public static Expression<Func<TClassType, object>> PropertyGetter<TClassType>(this PropertyInfo property) => property.PropertyGetter<TClassType, object>();

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <param name="expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string PropertyName(this LambdaExpression expression)
        {
            if (expression is null)
            {
                return "";
            }

            if (expression.Body is UnaryExpression && expression.Body.NodeType == ExpressionType.Convert)
            {
                var Temp = (MemberExpression)((UnaryExpression)expression.Body).Operand;
                return Temp.Expression.PropertyName() + Temp.Member.Name;
            }
            if (!(expression.Body is MemberExpression))
            {
                throw new ArgumentException(Resources.NotAMemberExpression);
            }

            return ((MemberExpression)expression.Body).Expression.PropertyName() + ((MemberExpression)expression.Body).Member.Name;
        }

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <param name="expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        public static string PropertyName(this Expression expression)
        {
            return !(expression is MemberExpression TempExpression)
                ? ""
                : TempExpression.Expression.PropertyName() + TempExpression.Member.Name + ".";
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="TClassType">Class type</typeparam>
        /// <typeparam name="TDataType">Data type expecting</typeparam>
        /// <param name="property">Property</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        public static Expression<Action<TClassType, TDataType>>? PropertySetter<TClassType, TDataType>(this LambdaExpression property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var TempPropertyName = property.PropertyName();
            var SplitName = TempPropertyName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (SplitName.Length == 0)
            {
                return null;
            }

            var PropertyInfo = typeof(TClassType).GetProperty<TClassType>(SplitName[0]);
            var ObjectInstance = Expression.Parameter(PropertyInfo?.DeclaringType, "x");
            var PropertySet = Expression.Parameter(typeof(TDataType), "y");
            var DefaultConstant = Expression.Constant(((object?)null).To(PropertyInfo?.PropertyType!, null), PropertyInfo?.PropertyType);
            MethodCallExpression? SetterCall = null;
            MemberExpression? PropertyGet = null;
            if (SplitName.Length > 1)
            {
                PropertyGet = Expression.Property(ObjectInstance, PropertyInfo);
                for (var x = 1; x < SplitName.Length - 1; ++x)
                {
                    PropertyInfo = PropertyInfo?.PropertyType.GetProperty(SplitName[x], true);
                    if (PropertyInfo is null)
                    {
                        throw new NullReferenceException(Resources.PropertyInfoCantBeNull);
                    }

                    PropertyGet = Expression.Property(PropertyGet, PropertyInfo);
                }
                PropertyInfo = PropertyInfo?.PropertyType.GetProperty(SplitName[^1], true);
            }
            var SetMethod = PropertyInfo?.GetSetMethod();
            if (!(SetMethod is null))
            {
                if (PropertyInfo?.PropertyType != typeof(TDataType))
                {
                    var ConversionMethod = Array.Find(typeof(GenericObjectExtensions).GetMethods(), x => x.ContainsGenericParameters
                        && x.GetGenericArguments().Length == 2
                        && x.Name == "To"
                        && x.GetParameters().Length == 2);
                    ConversionMethod = ConversionMethod.MakeGenericMethod(typeof(TDataType), PropertyInfo?.PropertyType);
                    var Convert = Expression.Call(ConversionMethod, PropertySet, DefaultConstant);
                    SetterCall = PropertyGet is null ? Expression.Call(ObjectInstance, SetMethod, Convert) : Expression.Call(PropertyGet, SetMethod, Convert);
                    return Expression.Lambda<Action<TClassType, TDataType>>(SetterCall, ObjectInstance, PropertySet);
                }
                SetterCall = PropertyGet is null ? Expression.Call(ObjectInstance, SetMethod, PropertySet) : Expression.Call(PropertyGet, SetMethod, PropertySet);
            }
            else
            {
                return Expression.Lambda<Action<TClassType, TDataType>>(Expression.Empty(), ObjectInstance, PropertySet);
            }

            return Expression.Lambda<Action<TClassType, TDataType>>(SetterCall, ObjectInstance, PropertySet);
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="TClassType">Class type</typeparam>
        /// <param name="property">Property</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        public static Expression<Action<TClassType, object>>? PropertySetter<TClassType>(this LambdaExpression property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return property.PropertySetter<TClassType, object>();
        }

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="inputObject">object who contains the property</param>
        /// <param name="propertyPath">
        /// Path of the property (ex: Prop1.Prop2.Prop3 would be the Prop1 of the source object,
        /// which then has a Prop2 on it, which in turn has a Prop3 on it.)
        /// </param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type? PropertyType(this object inputObject, string propertyPath) => inputObject is null || string.IsNullOrEmpty(propertyPath) ? null : inputObject.GetType().PropertyType(propertyPath);

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="propertyPath">
        /// Path of the property (ex: Prop1.Prop2.Prop3 would be the Prop1 of the source object,
        /// which then has a Prop2 on it, which in turn has a Prop3 on it.)
        /// </param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        public static Type? PropertyType(this Type? objectType, string propertyPath)
        {
            if (objectType is null || string.IsNullOrEmpty(propertyPath))
            {
                return null;
            }
            Type? TempObject = objectType;

            var SourceProperties = propertyPath.Split(new string[] { "." }, StringSplitOptions.None);
            for (var x = 0; x < SourceProperties.Length; ++x)
            {
                TempObject = TempObject.GetProperty(SourceProperties[x], true)?.PropertyType;
            }
            return TempObject;
        }

        /// <summary>
        /// Gets the version information in a string format
        /// </summary>
        /// <param name="assembly">Assembly to get version information from</param>
        /// <param name="infoType">Version info type</param>
        /// <returns>The version information as a string</returns>
        public static string ToString(this Assembly assembly, VersionInfo infoType)
        {
            if (assembly is null)
            {
                return "";
            }

            if ((infoType & VersionInfo.ShortVersion) != 0)
            {
                var Version = assembly.GetName().Version;
                return Version.Major + "." + Version.Minor;
            }
            else
            {
                return assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets the version information in a string format
        /// </summary>
        /// <param name="assemblies">Assemblies to get version information from</param>
        /// <param name="infoType">Version info type</param>
        /// <returns>The version information as a string</returns>
        public static string ToString(this IEnumerable<Assembly> assemblies, VersionInfo infoType)
        {
            if (assemblies?.Any() != true)
            {
                return "";
            }

            var Builder = new StringBuilder();
            assemblies.OrderBy(x => x.FullName).ForEach<Assembly>(x => Builder.Append(x.GetName().Name).Append(": ").AppendLine(x.ToString(infoType)));
            return Builder.ToString();
        }

        /// <summary>
        /// Gets assembly information for all currently loaded assemblies
        /// </summary>
        /// <param name="assemblies">Assemblies to dump information from</param>
        /// <param name="htmlOutput">Should HTML output be used</param>
        /// <returns>An HTML formatted string containing the assembly information</returns>
        public static string ToString(this IEnumerable<Assembly> assemblies, bool htmlOutput)
        {
            if (assemblies?.Any() != true)
            {
                return "";
            }

            var Builder = new StringBuilder();
            Builder.Append(htmlOutput ? "<strong>Assembly Information</strong><br />" : "Assembly Information\r\n");
            assemblies.ForEach<Assembly>(x => Builder.Append(x.ToString(htmlOutput)));
            return Builder.ToString();
        }

        /// <summary>
        /// Dumps the property names and current values from an object
        /// </summary>
        /// <param name="inputObject">Object to dunp</param>
        /// <param name="htmlOutput">Determines if the output should be HTML or not</param>
        /// <returns>An HTML formatted table containing the information about the object</returns>
        public static string ToString(this object inputObject, bool htmlOutput)
        {
            if (inputObject is null)
            {
                return "";
            }

            var TempValue = new StringBuilder();
            TempValue.Append(htmlOutput ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>" : "Property Name\t\t\t\tProperty Value");
            var ObjectType = inputObject.GetType();
            var TempProperties = ObjectType.GetProperties();
            for (int x = 0, TempPropertiesLength = TempProperties.Length; x < TempPropertiesLength; x++)
            {
                var TempProperty = TempProperties[x];
                TempValue.Append(htmlOutput ? "<tr><td>" : Environment.NewLine).Append(TempProperty.Name).Append(htmlOutput ? "</td><td>" : "\t\t\t\t");
                var Parameters = TempProperty.GetIndexParameters();
                if (TempProperty.CanRead && Parameters.Length == 0)
                {
                    try
                    {
                        var Value = TempProperty.GetValue(inputObject, null);
                        TempValue.Append(Value is null ? "null" : Value.ToString());
                    }
                    catch { }
                }
                TempValue.Append(htmlOutput ? "</td></tr>" : "");
            }

            TempValue.Append(htmlOutput ? "</tbody></table>" : "");
            return TempValue.ToString();
        }

        /// <summary>
        /// Dumps the properties names and current values from an object type (used for static classes)
        /// </summary>
        /// <param name="objectType">Object type to dunp</param>
        /// <param name="htmlOutput">Should this be output as an HTML string</param>
        /// <returns>An HTML formatted table containing the information about the object type</returns>
        public static string ToString(this Type objectType, bool htmlOutput)
        {
            if (objectType is null)
            {
                return "";
            }

            var TempValue = new StringBuilder();
            TempValue.Append(htmlOutput ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>" : "Property Name\t\t\t\tProperty Value");
            var Properties = objectType.GetProperties();
            for (int x = 0, PropertiesLength = Properties.Length; x < PropertiesLength; x++)
            {
                var TempProperty = Properties[x];
                TempValue.Append(htmlOutput ? "<tr><td>" : Environment.NewLine).Append(TempProperty.Name).Append(htmlOutput ? "</td><td>" : "\t\t\t\t");
                if (TempProperty.CanRead && TempProperty.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        TempValue.Append(TempProperty.GetValue(null, null) is null ? "null" : TempProperty.GetValue(null, null).ToString());
                    }
                    catch { }
                }
                TempValue.Append(htmlOutput ? "</td></tr>" : "");
            }

            TempValue.Append(htmlOutput ? "</tbody></table>" : "");
            return TempValue.ToString();
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="assembly">Assembly to check</param>
        /// <typeparam name="TBaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types<TBaseType>(this Assembly assembly) => assembly?.Types(typeof(TBaseType)) ?? Array.Empty<Type>();

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="assembly">Assembly to check</param>
        /// <param name="baseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types(this Assembly assembly, Type baseType)
        {
            if (assembly is null || baseType is null)
            {
                return Array.Empty<Type>();
            }

            try
            {
                return assembly.GetTypes().Where(x => x.Is(baseType) && x.IsClass && !x.IsAbstract);
            }
            catch { return Array.Empty<Type>(); }
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="assemblies">Assemblies to check</param>
        /// <typeparam name="TBaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types<TBaseType>(this IEnumerable<Assembly> assemblies) => assemblies?.Any() != true ? Array.Empty<Type>() : assemblies.Types(typeof(TBaseType));

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="assemblies">Assemblies to check</param>
        /// <param name="baseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types(this IEnumerable<Assembly> assemblies, Type baseType)
        {
            if (assemblies?.Any() != true || baseType is null)
            {
                yield break;
            }

            foreach (var Assembly in assemblies)
            {
                foreach (var Type in Assembly.Types(baseType))
                {
                    yield return Type;
                }
            }
        }

        /// <summary>
        /// Gets a list of types in the assemblies specified
        /// </summary>
        /// <param name="assemblies">Assemblies to check</param>
        /// <returns>List of types</returns>
        public static IEnumerable<Type> Types(this IEnumerable<Assembly> assemblies)
        {
            if (assemblies?.Any() != true)
            {
                yield break;
            }

            foreach (var Assembly in assemblies)
            {
                foreach (var Type in Assembly.GetTypes())
                {
                    yield return Type;
                }
            }
        }

        /// <summary>
        /// Finds the type of the IEnumerable element.
        /// </summary>
        /// <param name="type">The original type.</param>
        /// <returns>Either null or the type of the IEnumerable</returns>
        private static Type? FindIEnumerableElementType(Type type)
        {
            if (type is null || type == typeof(string))
            {
                return null;
            }

            if (type.IsArray)
            {
                return typeof(IEnumerable<>).MakeGenericType(type.GetElementType());
            }

            var TypeInfo = type;
            if (TypeInfo.IsGenericType)
            {
                var maxLength = type.GetGenericArguments().Length;
                for (var x = 0; x < maxLength; ++x)
                {
                    var Arg = type.GetGenericArguments()[x];
                    var IEnum = typeof(IEnumerable<>).MakeGenericType(Arg);

                    if (IEnum.IsAssignableFrom(type))
                    {
                        return IEnum;
                    }
                }
            }

            var Interfaces = type.GetInterfaces();
            if (Interfaces?.Length > 0)
            {
                var InterfacesLength = Interfaces.Length;
                for (var x = 0; x < InterfacesLength; ++x)
                {
                    var InterfaceUsed = Interfaces[x];
                    var IEnum = FindIEnumerableElementType(InterfaceUsed);

                    if (!(IEnum is null))
                    {
                        return IEnum;
                    }
                }
            }

            return !(TypeInfo.BaseType is null) && TypeInfo.BaseType != typeof(object) ?
                FindIEnumerableElementType(TypeInfo.BaseType) :
                null;
        }

        /// <summary>
        /// Determines whether [is JIT optimizer disabled].
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>
        /// <c>true</c> if [is JIT optimizer disabled] [the specified attribute]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsJitOptimized(this DebuggableAttribute attribute) => !(bool)(attribute.Property("IsJITOptimizerDisabled") ?? true);

        /// <summary>
        /// Determines whether [is JIT tracking enabled].
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>
        /// <c>true</c> if [is JIT tracking enabled] [the specified attribute]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsJitTrackingEnabled(this DebuggableAttribute attribute) => (bool)(attribute.Property("IsJITTrackingEnabled") ?? false);
    }
}