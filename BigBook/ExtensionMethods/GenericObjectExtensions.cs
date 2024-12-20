﻿/*
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

using BigBook.Comparison;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace BigBook
{
    /// <summary>
    /// Generic extensions dealing with objects
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class GenericObjectExtensions
    {
        /// <summary>
        /// Checks to see if the object meets all the criteria. If it does, it returns the object.
        /// If it does not, it returns the default object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="inputObject">Object to check</param>
        /// <param name="predicate">Predicate to check the object against</param>
        /// <param name="defaultValue">The default value to return</param>
        /// <returns>The default object if it fails the criteria, the object otherwise</returns>
        public static T Check<T>(this T inputObject, Predicate<T> predicate, T defaultValue = default)
        {
            if (predicate is null)
                return inputObject;

            return predicate(inputObject) ? inputObject : defaultValue;
        }

        /// <summary>
        /// Checks to see if the object meets all the criteria. If it does, it returns the object.
        /// If it does not, it returns the default object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="inputObject">Object to check</param>
        /// <param name="predicate">Predicate to check the object against</param>
        /// <param name="defaultValue">The default value to return</param>
        /// <returns>The default object if it fails the criteria, the object otherwise</returns>
        public static T Check<T>(this T inputObject, Predicate<T> predicate, Func<T> defaultValue)
        {
            if (predicate is null || defaultValue is null)
                return inputObject;

            return predicate(inputObject) ? inputObject : defaultValue();
        }

        /// <summary>
        /// Checks to see if the object is null. If it is, it returns the default object, otherwise
        /// the object is returned.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="inputObject">Object to check</param>
        /// <param name="defaultValue">The default value to return</param>
        /// <returns>The default object if it is null, the object otherwise</returns>
        public static T Check<T>(this T inputObject, T defaultValue = default) => inputObject.Check(x => !Equals(x, default(T)!), defaultValue);

        /// <summary>
        /// Checks to see if the object is null. If it is, it returns the default object, otherwise
        /// the object is returned.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="inputObject">Object to check</param>
        /// <param name="defaultValue">The default value to return</param>
        /// <returns>The default object if it is null, the object otherwise</returns>
        public static T Check<T>(this T inputObject, Func<T> defaultValue)
        {
            if (defaultValue is null)
                return inputObject;

            return inputObject.Check(x => !Equals(x, default(T)!), defaultValue);
        }

        /// <summary>
        /// Executes a function, repeating it a number of times in case it fails
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="function">Function to run</param>
        /// <param name="attempts">Number of times to attempt it</param>
        /// <param name="retryDelay">The amount of milliseconds to wait between tries</param>
        /// <param name="timeOut">
        /// Max amount of time to wait for the function to run (waits for the current attempt to
        /// finish before checking)
        /// </param>
        /// <returns>The returned value from the function</returns>
        [return: MaybeNull]
        public static T Execute<T>(this Func<T> function, int attempts = 3, int retryDelay = 0, int timeOut = int.MaxValue)
        {
            if (function is null)
                return default!;

            Exception? Holder = null;
            long Start = Environment.TickCount;
            while (attempts > 0)
            {
                try
                {
                    return function();
                }
                catch (Exception e) { Holder = e; }
                if (Environment.TickCount - Start > timeOut)
                {
                    break;
                }

                Thread.Sleep(retryDelay);
                --attempts;
            }
            if (!(Holder is null))
            {
                throw Holder;
            }

            return default!;
        }

        /// <summary>
        /// Executes an action, repeating it a number of times in case it fails
        /// </summary>
        /// <param name="action">Action to run</param>
        /// <param name="attempts">Number of times to attempt it</param>
        /// <param name="retryDelay">The amount of milliseconds to wait between tries</param>
        /// <param name="timeOut">
        /// Max amount of time to wait for the function to run (waits for the current attempt to
        /// finish before checking)
        /// </param>
        /// <returns>True if it is executed successfully, false otherwise</returns>
        public static bool Execute(this Action action, int attempts = 3, int retryDelay = 0, int timeOut = int.MaxValue)
        {
            if (action is null)
                return false;

            Exception? Holder = null;
            long Start = Environment.TickCount;
            while (attempts > 0)
            {
                try
                {
                    action();
                    return true;
                }
                catch (Exception e) { Holder = e; }
                if (Environment.TickCount - Start > timeOut)
                {
                    break;
                }

                Thread.Sleep(retryDelay);
                --attempts;
            }
            if (!(Holder is null))
            {
                throw Holder;
            }

            return false;
        }

        /// <summary>
        /// Calls the object's ToString function passing in the formatting
        /// </summary>
        /// <param name="input">Input object</param>
        /// <param name="format">Format of the output string</param>
        /// <returns>The formatted string</returns>
        public static string FormatToString(this object input, string format)
        {
            if (input is null)
                return string.Empty;

            return !string.IsNullOrEmpty(format) ? input.Call<string>("ToString", format) : input.ToString();
        }

        /// <summary>
        /// Determines if the object passes the predicate passed in
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="inputObject">Object to test</param>
        /// <param name="predicate">Predicate to test</param>
        /// <returns>True if the object passes the predicate, false otherwise</returns>
        public static bool Is<T>(this T inputObject, Predicate<T> predicate) => !(predicate is null) && predicate(inputObject);

        /// <summary>
        /// Determines if the object is equal to a specific value
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="inputObject">Object to test</param>
        /// <param name="comparisonObject">Comparison object</param>
        /// <param name="comparer">Comparer</param>
        /// <returns>True if the object passes the predicate, false otherwise</returns>
        public static bool Is<T>(this T inputObject, T comparisonObject, IEqualityComparer<T>? comparer = null)
        {
            comparer ??= GenericEqualityComparer<T>.Comparer;
            return comparer.Equals(inputObject, comparisonObject);
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="item">The item</param>
        /// <param name="predicate">Predicate to check</param>
        /// <param name="exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static T ThrowIf<T>(this T item, Predicate<T> predicate, Func<Exception> exception)
        {
            if (predicate is null)
                return item;

            if (predicate(item))
                throw exception();

            return item;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="item">The item</param>
        /// <param name="predicate">Predicate to check</param>
        /// <param name="exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static T ThrowIf<T>(this T item, Predicate<T> predicate, Exception exception)
        {
            if (predicate is null)
                return item;

            if (predicate(item))
                throw exception;

            return item;
        }

        /// <summary>
        /// Determines if the object is equal to default value and throws an ArgumentNullException
        /// if it is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="name">Name of the argument</param>
        /// <param name="equalityComparer">
        /// Equality comparer used to determine if the object is equal to default
        /// </param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfDefault<T>(this T item, string name, IEqualityComparer<T>? equalityComparer = null) => item.ThrowIfDefault(new ArgumentNullException(name), equalityComparer);

        /// <summary>
        /// Determines if the object is equal to default value and throws the exception that is
        /// passed in if it is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="exception">Exception to throw</param>
        /// <param name="equalityComparer">
        /// Equality comparer used to determine if the object is equal to default
        /// </param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfDefault<T>(this T item, Exception exception, IEqualityComparer<T>? equalityComparer = null) => item.ThrowIf(x => (equalityComparer ?? GenericEqualityComparer<T>.Comparer).Equals(x, default!), exception);

        /// <summary>
        /// Throws the specified exception if the predicate is false for the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="item">The item</param>
        /// <param name="predicate">Predicate to check</param>
        /// <param name="exception">Exception to throw if predicate is false</param>
        /// <returns>the original Item</returns>
        public static T ThrowIfNot<T>(this T item, Predicate<T> predicate, Exception exception) => item.ThrowIf(x => !predicate(x), exception);

        /// <summary>
        /// Determines if the object is not equal to default value and throws an ArgumentException
        /// if it is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="name">Name of the argument</param>
        /// <param name="equalityComparer">
        /// Equality comparer used to determine if the object is equal to default
        /// </param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNotDefault<T>(this T item, string name, IEqualityComparer<T>? equalityComparer = null) => item.ThrowIfNotDefault(new ArgumentException(name), equalityComparer);

        /// <summary>
        /// Determines if the object is not equal to default value and throws the exception that is
        /// passed in if it is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="exception">Exception to throw</param>
        /// <param name="equalityComparer">
        /// Equality comparer used to determine if the object is equal to default
        /// </param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNotDefault<T>(this T item, Exception exception, IEqualityComparer<T>? equalityComparer = null) => item.ThrowIf(x => !(equalityComparer ?? GenericEqualityComparer<T>.Comparer).Equals(x, default!), exception);

        /// <summary>
        /// Determines if the object is not null and throws an ArgumentException if it is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNotNull<T>(this T item, string name)
            where T : class => item.ThrowIfNotNull(new ArgumentException(name));

        /// <summary>
        /// Determines if the object is not null and throws the exception passed in if it is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="exception">Exception to throw</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNotNull<T>(this T item, Exception exception)
            where T : class => item.ThrowIf(x => !(x is null) && x != DBNull.Value, exception);

        /// <summary>
        /// Determines if the IEnumerable is not null or empty and throws an ArgumentException if it is
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static IEnumerable<T> ThrowIfNotNullOrEmpty<T>(this IEnumerable<T> item, string name) => item.ThrowIfNotNullOrEmpty(new ArgumentException(name));

        /// <summary>
        /// Determines if the IEnumerable is not null or empty and throws the exception passed in if
        /// it is
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="exception">Exception to throw</param>
        /// <returns>Returns Item</returns>
        public static IEnumerable<T> ThrowIfNotNullOrEmpty<T>(this IEnumerable<T> item, Exception exception) => item.ThrowIf(x => x?.Any() == true, exception);

        /// <summary>
        /// Determines if the object is null and throws an ArgumentNullException if it is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNull<T>(this T item, string name)
            where T : class => item.ThrowIfNull(new ArgumentNullException(name));

        /// <summary>
        /// Determines if the object is null and throws the exception passed in if it is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="exception">Exception to throw</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNull<T>(this T item, Exception exception)
            where T : class => item.ThrowIf(x => x is null || x == DBNull.Value, exception);

        /// <summary>
        /// Determines if the IEnumerable is null or empty and throws an ArgumentNullException if it is
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> item, string name) => item.ThrowIfNullOrEmpty(new ArgumentNullException(name));

        /// <summary>
        /// Determines if the IEnumerable is null or empty and throws the exception passed in if it is
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="item">The object to check</param>
        /// <param name="exception">Exception to throw</param>
        /// <returns>Returns Item</returns>
        public static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> item, Exception exception) => item.ThrowIf(x => x?.Any() != true, exception);

        /// <summary>
        /// Runs a function based on the number of times specified and returns the results
        /// </summary>
        /// <typeparam name="T">Type that gets returned</typeparam>
        /// <param name="count">Number of times the function should run</param>
        /// <param name="function">The function that should run</param>
        /// <returns>The results from the function</returns>
        public static IEnumerable<T> Times<T>(this int count, Func<int, T> function)
        {
            if (function is null)
                yield break;

            for (var x = 0; x < count; ++x)
            {
                yield return function(x);
            }
        }

        /// <summary>
        /// Runs an action based on the number of times specified
        /// </summary>
        /// <param name="count">Number of times to run the action</param>
        /// <param name="action">Action to run</param>
        /// <returns>count</returns>
        public static int Times(this int count, Action<int> action)
        {
            if (action is null)
                return count;

            for (var x = 0; x < count; ++x)
            {
                action(x);
            }

            return count;
        }

        /// <summary>
        /// When the predicate is true, run the method.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="predicate">if set to <c>true</c> [predicate].</param>
        /// <param name="method">The method to run if true.</param>
        /// <returns>The return value for the method.</returns>
        public static TObject When<TObject>(
            this TObject obj,
            bool predicate,
            Func<TObject, TObject> method) => !(method is null) && predicate ? method(obj) : obj;

        /// <summary>
        /// When the predicate is true, run the method.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="predicate">if set to <c>true</c> [predicate].</param>
        /// <param name="method">The method to run if true.</param>
        /// <param name="defaultValue">The default value if it's .</param>
        /// <returns>The return value</returns>
        public static TReturn When<TObject, TReturn>(
            this TObject obj,
            bool predicate,
            Func<TObject, TReturn> method,
            TReturn defaultValue = default!) => !(method is null) && predicate ? method(obj) : defaultValue;

        /// <summary>
        /// When the predicate is true, run the method.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="predicate">if set to <c>true</c> [predicate].</param>
        /// <param name="method">The method to run if true.</param>
        /// <returns>The return value for the method.</returns>
        public static TObject When<TObject>(
            this TObject obj,
            bool predicate,
            Action<TObject> method)
        {
            if (!(method is null) && predicate)
                method(obj);
            return obj;
        }
    }
}