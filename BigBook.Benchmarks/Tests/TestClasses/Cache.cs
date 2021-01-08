using System;
using System.Collections.Generic;
using System.Linq;

namespace BigBook.Benchmarks.Tests.TestClasses
{
    /// <summary>
    /// Built in cache
    /// </summary>
    public class Cache : CacheBase
    {
        /// <summary>
        /// The number of items in the cache
        /// </summary>
        public override int Count => InternalCache.Count;

        /// <summary>
        /// Keys
        /// </summary>
        public override ICollection<string> Keys => keys;

        /// <summary>
        /// Name
        /// </summary>
        public override string Name { get; } = "Default";

        /// <summary>
        /// Values
        /// </summary>
        public override ICollection<object> Values => InternalCache.Values;

        /// <summary>
        /// Internal cache
        /// </summary>
        protected Dictionary<int, object> InternalCache { get; } = new Dictionary<int, object>();

        /// <summary>
        /// The keys
        /// </summary>
        private List<string> keys = new List<string>();

        /// <summary>
        /// Determines if the item is in the cache
        /// </summary>
        /// <param name="item">item to check for</param>
        /// <returns></returns>
        public override bool Contains(KeyValuePair<string, object> item)
        {
            if (!InternalCache.TryGetValue(item.Key.GetHashCode(StringComparison.Ordinal), out var Value))
                return false;
            return Value.Equals(item.Value);
        }

        /// <summary>
        /// Checks if the cache contains the key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it is there, false otherwise</returns>
        public override bool ContainsKey(string key)
        {
            return InternalCache.ContainsKey(key.GetHashCode(StringComparison.Ordinal));
        }

        /// <summary>
        /// Copies to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public override void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            var Values = InternalCache.ToArray();
            for (int x = arrayIndex; x < array.Length; ++x)
            {
                array[x] = new KeyValuePair<string, object>(keys[x], Values[x].Value);
            }
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            int x = 0;
            foreach (var Item in InternalCache)
            {
                yield return new KeyValuePair<string, object>(keys[x], Item.Value);
                ++x;
            }
        }

        /// <summary>
        /// Disposes the cache
        /// </summary>
        /// <param name="Managed">Managed objects or just unmanaged</param>
        protected override void Dispose(bool Managed)
        {
            if (InternalCache is null)
                return;
            foreach (var Item in InternalCache.Values.OfType<IDisposable>())
            {
                Item.Dispose();
            }
            Keys.Clear();
            InternalCache.Clear();
        }

        /// <summary>
        /// Used internally to add items (a lock is already placed by this point in time).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void InternalAdd(string key, object value)
        {
            var KeyHash = key.GetHashCode(StringComparison.Ordinal);
            if (InternalCache.TryGetValue(KeyHash, out _))
                InternalCache[KeyHash] = value;
            else
            {
                Keys.Add(key);
                InternalCache.Add(KeyHash, value);
            }
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        protected override void InternalClear()
        {
            InternalCache.Clear();
            Keys.Clear();
        }

        /// <summary>
        /// The internal method called to remove an item. (a lock has already been placed by this point)
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if it is removed, false otherwise.</returns>
        protected override bool InternalRemove(string key)
        {
            var KeyHash = key.GetHashCode(StringComparison.Ordinal);
            if (!Keys.Remove(key))
                return false;
            return InternalCache.Remove(KeyHash);
        }

        /// <summary>
        /// Attempt to get a value
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="value">Value of the item</param>
        /// <returns>True if it is found, false otherwise</returns>
        protected override bool InternalTryGetValue(string key, out object value)
        {
            var KeyHash = key.GetHashCode(StringComparison.Ordinal);
            return InternalCache.TryGetValue(KeyHash, out value);
        }
    }
}