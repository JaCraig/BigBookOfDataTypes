using System;
using System.Collections.Generic;

namespace BigBook
{
    /// <summary>
    /// Two way, many to many index
    /// </summary>
    /// <typeparam name="TFirst">The type of the first.</typeparam>
    /// <typeparam name="TSecond">The type of the second.</typeparam>
    public class ManyToManyIndex<TFirst, TSecond>
        where TFirst : notnull
        where TSecond : notnull
    {
        /// <summary>
        /// Gets the first.
        /// </summary>
        /// <value>The first.</value>
        public IEnumerable<TFirst> First => FirstMapping.Keys;

        /// <summary>
        /// Gets the second.
        /// </summary>
        /// <value>The second.</value>
        public IEnumerable<TSecond> Second => SecondMapping.Keys;

        /// <summary>
        /// Gets the first mapping.
        /// </summary>
        /// <value>The first mapping.</value>
        private ListMapping<TFirst, TSecond> FirstMapping { get; } = new ListMapping<TFirst, TSecond>();

        /// <summary>
        /// Gets the second mapping.
        /// </summary>
        /// <value>The second mapping.</value>
        private ListMapping<TSecond, TFirst> SecondMapping { get; } = new ListMapping<TSecond, TFirst>();

        /// <summary>
        /// Adds the specified data to the mapping
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        public void Add(TFirst key, params TSecond[] list)
        {
            FirstMapping.Add(key, list);
            for (int x = 0; x < list.Length; ++x)
            {
                SecondMapping.Add(list[x], key);
            }
        }

        /// <summary>
        /// Adds the specified data to the mapping
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        public void Add(TSecond key, params TFirst[] list)
        {
            SecondMapping.Add(key, list);
            for (int x = 0; x < list.Length; ++x)
            {
                FirstMapping.Add(list[x], key);
            }
        }

        /// <summary>
        /// Adds the specified data to the mapping
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        public void Add(TFirst key, IEnumerable<TSecond> list)
        {
            list ??= Array.Empty<TSecond>();
            FirstMapping.Add(key, list);
            foreach (var Item in list)
            {
                SecondMapping.Add(Item, key);
            }
        }

        /// <summary>
        /// Adds the specified data to the mapping
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        public void Add(TSecond key, IEnumerable<TFirst> list)
        {
            list ??= Array.Empty<TFirst>();
            SecondMapping.Add(key, list);
            foreach (var Item in list)
            {
                FirstMapping.Add(Item, key);
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            SecondMapping.Clear();
            FirstMapping.Clear();
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(TFirst key)
        {
            if (!FirstMapping.TryGetValue(key, out var List))
                return false;
            foreach (var Item in List)
            {
                SecondMapping.Remove(Item, key);
            }
            FirstMapping.Remove(key);
            return true;
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(TSecond key)
        {
            if (!SecondMapping.TryGetValue(key, out var List))
                return false;
            foreach (var Item in List)
            {
                FirstMapping.Remove(Item, key);
            }
            SecondMapping.Remove(key);
            return true;
        }

        /// <summary>
        /// Tries to get the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns>True if it is returned, false otherwise.</returns>
        public bool TryGetValue(TFirst key, out IEnumerable<TSecond> values)
        {
            return FirstMapping.TryGetValue(key, out values);
        }

        /// <summary>
        /// Tries to get the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns>True if it is returned, false otherwise.</returns>
        public bool TryGetValue(TSecond key, out IEnumerable<TFirst> values)
        {
            return SecondMapping.TryGetValue(key, out values);
        }
    }
}