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

using BigBook.DataMapper;
using BigBook.DataMapper.Interfaces;
using BigBook.DynamoUtils;
using BigBook.Reflection;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace BigBook
{
    /// <summary>
    /// Dynamic object implementation (used when inheriting)
    /// </summary>
    /// <typeparam name="TClass">Child object type</typeparam>
    public abstract class Dynamo<TClass> : Dynamo
        where TClass : Dynamo<TClass>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected Dynamo()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item">Item to copy values from</param>
        /// <param name="useChangeLog">if set to <c>true</c> [use change log].</param>
        /// <param name="aopManager">
        /// The aop manager (if available it will attempt to use this to create an object).
        /// </param>
        /// <param name="builderPool">
        /// The builder pool (if available, it will use a StringBuilder from the pool where applicable).
        /// </param>
        /// <param name="dataMapper">
        /// The data mapper (if available, will use this to map itself to other data types when
        /// copying data).
        /// </param>
        protected Dynamo(object? item, bool useChangeLog = false, Aspectus.Aspectus? aopManager = null, ObjectPool<StringBuilder>? builderPool = null, Manager? dataMapper = null)
            : base(item, useChangeLog, aopManager, builderPool, dataMapper)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="useChangeLog">if set to <c>true</c> [use change log].</param>
        /// <param name="aopManager">
        /// The aop manager (if available it will attempt to use this to create an object).
        /// </param>
        /// <param name="builderPool">
        /// The builder pool (if available, it will use a StringBuilder from the pool where applicable).
        /// </param>
        /// <param name="dataMapper">
        /// The data mapper (if available, will use this to map itself to other data types when
        /// copying data).
        /// </param>
        protected Dynamo(bool useChangeLog, Aspectus.Aspectus? aopManager = null, ObjectPool<StringBuilder>? builderPool = null, Manager? dataMapper = null)
            : base(useChangeLog, aopManager, builderPool, dataMapper)
        {
        }

        /// <summary>
        /// Keys to the dynamic type
        /// </summary>
        public override ICollection<string> Keys
        {
            get
            {
                var Temp = new List<string>
                {
                    base.Keys
                };
                foreach (var Property in TypeCacheFor<TClass>.Properties.Where(x => x.DeclaringType != typeof(Dynamo<TClass>) && x.DeclaringType != typeof(Dynamo)))
                {
                    Temp.Add(Property.Name);
                }
                return Temp;
            }
        }

        /// <summary>
        /// Gets the Values
        /// </summary>
        public override ICollection<object?> Values
        {
            get
            {
                var TempKeys = Keys;
                var Temp = new List<object?>(TempKeys.Count);
                foreach (var Key in TempKeys)
                {
                    Temp.Add(GetValue(Key, typeof(object)));
                }
                return Temp;
            }
        }
    }

    /// <summary>
    /// Dynamic object implementation
    /// </summary>
    /// <seealso cref="Dynamo"/>
    public class Dynamo : DynamicObject, IDictionary<string, object?>, INotifyPropertyChanged
    {
        /// <summary>
        /// Creates a new ExpandoObject with no members.
        /// </summary>
        public Dynamo()
        {
            Data = DynamoData.Empty;
            LockObject = new object();
            TypeInfo?.SetupType(this);
            HashCode = EmptyHashCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dynamo"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="useChangeLog">if set to <c>true</c> [use change log].</param>
        /// <param name="aopManager">
        /// The aop manager (if available it will attempt to use this to create an object).
        /// </param>
        /// <param name="builderPool">
        /// The builder pool (if available, it will use a StringBuilder from the pool where applicable).
        /// </param>
        /// <param name="dataMapper">
        /// The data mapper (if available, will use this to map itself to other data types when
        /// copying data).
        /// </param>
        public Dynamo(object? item, bool useChangeLog = false, Aspectus.Aspectus? aopManager = null, ObjectPool<StringBuilder>? builderPool = null, Manager? dataMapper = null)
            : this(useChangeLog, aopManager, builderPool, dataMapper)
        {
            Copy(item);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="useChangeLog">if set to <c>true</c> [use change log].</param>
        /// <param name="aopManager">
        /// The aop manager (if available it will attempt to use this to create an object).
        /// </param>
        /// <param name="builderPool">
        /// The builder pool (if available, it will use a StringBuilder from the pool where applicable).
        /// </param>
        /// <param name="dataMapper">
        /// The data mapper (if available, will use this to map itself to other data types when
        /// copying data).
        /// </param>
        public Dynamo(bool useChangeLog, Aspectus.Aspectus? aopManager = null, ObjectPool<StringBuilder>? builderPool = null, Manager? dataMapper = null)
            : this()
        {
            ChangeLog = useChangeLog ? new ConcurrentDictionary<string, Change>() : null;
            AOPManager = aopManager;
            BuilderPool = builderPool;
            DataMapper = dataMapper ?? new Manager(new IDataMapper[] { new DataMapper.Default.DataMapper() }, Array.Empty<IMapperModule>());
        }

        /// <summary>
        /// Gets the change log.
        /// </summary>
        /// <value>The change log.</value>
        public ConcurrentDictionary<string, Change>? ChangeLog { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        internal DynamoClass Definition => Data.Definition;

        /// <summary>
        /// Gets a value indicating whether the <see
        /// cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        public bool IsReadOnly { get; }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of
        /// the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        public virtual ICollection<string> Keys
        {
            get
            {
                var TempData = Data;
                string[] Result;
                lock (LockObject)
                {
                    Result = TempData.Definition.Keys.Select(x => x).ToArray();
                }
                return Result;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values
        /// in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        public virtual ICollection<object?> Values
        {
            get
            {
                var TempData = Data;
                object?[] Result;
                lock (LockObject)
                {
                    Result = TempData.Data.Where(x => x != UninitializedObject).ToArray();
                }
                return Result;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        internal DynamoData Data { get; private set; }

        /// <summary>
        /// Gets or sets the aop manager.
        /// </summary>
        /// <value>The aop manager.</value>
        private Aspectus.Aspectus? AOPManager { get; }

        /// <summary>
        /// Gets the builder pool.
        /// </summary>
        /// <value>The builder pool.</value>
        private ObjectPool<StringBuilder>? BuilderPool { get; }

        /// <summary>
        /// Gets or sets the data mapper.
        /// </summary>
        /// <value>The data mapper.</value>
        private Manager? DataMapper { get; set; }

        /// <summary>
        /// Gets or sets the hash code.
        /// </summary>
        /// <value>The hash code.</value>
        private int HashCode { get; set; }

        /// <summary>
        /// The uninitialized object
        /// </summary>
        internal static readonly object UninitializedObject = new object();

        /// <summary>
        /// The lock object
        /// </summary>
        internal readonly object LockObject;

        /// <summary>
        /// The empty hash code
        /// </summary>
        private const int EmptyHashCode = 6551;

        /// <summary>
        /// Gets the type information.
        /// </summary>
        /// <value>The type information.</value>
        private static DynamoTypes? TypeInfo = new DynamoTypes();

        /// <summary>
        /// The get value end_
        /// </summary>
        private Action<Dynamo, string, EventArgs.OnEndEventArgs>? getValueEnd_;

        /// <summary>
        /// The get value start_
        /// </summary>
        private Action<Dynamo, EventArgs.OnStartEventArgs>? getValueStart_;

        /// <summary>
        /// The property changed_
        /// </summary>
        private PropertyChangedEventHandler? propertyChanged_;

        /// <summary>
        /// Gets or sets the <see cref="object"/> with the specified key.
        /// </summary>
        /// <value>The <see cref="object"/>.</value>
        /// <param name="key">The key.</param>
        /// <returns>The object specified.</returns>
        public object? this[string key]
        {
            get
            {
                return GetValue(key, typeof(object));
            }
            set
            {
                TrySetValue(key, value);
            }
        }

        /// <summary>
        /// Called when the value/property is found but before it is returned to the caller Sends
        /// (this, PropertyName, EventArgs) to items attached to the event
        /// </summary>
        public event Action<Dynamo, string, EventArgs.OnEndEventArgs> GetValueEnd
        {
            add
            {
                getValueEnd_ -= value;
                getValueEnd_ += value;
            }
            remove
            {
                getValueEnd_ -= value;
            }
        }

        /// <summary>
        /// Called when beginning to get a value/property Sends (this, EventArgs) to items attached
        /// to the event
        /// </summary>
        public event Action<Dynamo, EventArgs.OnStartEventArgs> GetValueStart
        {
            add
            {
                getValueStart_ -= value;
                getValueStart_ += value;
            }
            remove
            {
                getValueStart_ -= value;
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                propertyChanged_ -= value;
                propertyChanged_ += value;
            }
            remove
            {
                propertyChanged_ -= value;
            }
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(string key, object? value)
        {
            TrySetValue(key, value);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public void Add(KeyValuePair<string, object?> item)
        {
            TrySetValue(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        public void Clear()
        {
            DynamoData TempData;
            lock (LockObject)
            {
                TempData = Data;
                Data = DynamoData.Empty;
                Count = 0;
            }
            for (var x = 0; x < TempData.Definition.Keys.Length; ++x)
            {
                var OldData = TempData[x];
                if (OldData != UninitializedObject)
                {
                    RaisePropertyChanged(TempData.Definition.Keys[x], OldData, null);
                }
            }
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="item"/> is found in the <see
        /// cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(KeyValuePair<string, object?> item)
        {
            return TryGetValue(item.Key, out var Value) && Equals(Value, item.Value);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains
        /// an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="T:System.Collections.Generic.IDictionary`2"/>
        /// contains an element with the key; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            var TempData = Data;
            var Index = TempData.Definition.GetIndex(key);
            return Index >= 0 && TempData.Data[Index] != UninitializedObject;
        }

        /// <summary>
        /// Copies the properties from an item
        /// </summary>
        /// <param name="item">Item to copy from</param>
        public void Copy(object? item)
        {
            if (item is null)
            {
                return;
            }

            var ItemType = item.GetType();
            if (item is string || ItemType.IsValueType)
            {
                TrySetValue("Value", item);
            }
            else if (item is IDictionary<string, object?> DictItem)
            {
                foreach (var Item in DictItem)
                {
                    TrySetValue(Item.Key, Item.Value);
                }
            }
            else if (item is IEnumerable)
            {
                TrySetValue("Items", item);
            }
            else
            {
                DataMapper?.Map(ItemType, GetType())
                          .AutoMap()
                          .Copy(item, this);
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to
        /// an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements
        /// copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see
        /// cref="T:System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">arrayIndex</exception>
        public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
        {
            if (array is null)
                return;

            if (arrayIndex < 0)
                arrayIndex = 0;
            if (arrayIndex > array.Length - Keys.Count)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            lock (LockObject)
            {
                foreach (var item in this)
                {
                    array[arrayIndex++] = item;
                }
            }
        }

        /// <summary>
        /// Copies data from here to another object
        /// </summary>
        /// <param name="result">Result</param>
        public void CopyTo(object? result)
        {
            if (result is null)
            {
                return;
            }
            DataMapper?.Map(GetType(), result.GetType())
                      .AutoMap()
                      .Copy(this, result);
        }

        /// <summary>
        /// Determines if two objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they're equal, false otherwise</returns>
        public override bool Equals(object? obj) => (obj is Dynamo TempObj) && TempObj.GetHashCode() == GetHashCode();

        /// <summary>
        /// Gets the dynamic member names
        /// </summary>
        /// <returns>The keys used internally</returns>
        public override IEnumerable<string> GetDynamicMemberNames() => Keys;

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        {
            var TempData = Data;
            for (var x = 0; x < TempData.Definition.Keys.Length; ++x)
            {
                var Value = TempData[x];
                if (Value != UninitializedObject)
                {
                    yield return new KeyValuePair<string, object?>(TempData.Definition.Keys[x], Value);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
        /// through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode() => HashCode;

        /// <summary>
        /// Not used
        /// </summary>
        /// <returns>Null</returns>
        public System.Xml.Schema.XmlSchema? GetSchema() => null;

        /// <summary>
        /// Reads the data from an XML doc
        /// </summary>
        /// <param name="reader">XML reader</param>
        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader is null)
                return;
            TrySetValue(reader.Name, reader.Value);
            while (reader.Read())
            {
                TrySetValue(reader.Name, reader.Value);
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// <see langword="true"/> if the element is successfully removed; otherwise, <see
        /// langword="false"/>. This method also returns <see langword="false"/> if <paramref
        /// name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public bool Remove(string key)
        {
            DynamoData TempData;
            object? OldValue;
            lock (LockObject)
            {
                TempData = Data;
                var Index = TempData.Definition.GetIndex(key);
                if (Index == -1)
                    return false;
                OldValue = TempData[Index];
                if (OldValue == UninitializedObject)
                    return false;
                TempData[Index] = UninitializedObject;
                --Count;
            }
            RaisePropertyChanged(key, OldValue, null);
            return true;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="item"/> was successfully removed from the <see
        /// cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, <see langword="false"/>.
        /// This method also returns <see langword="false"/> if <paramref name="item"/> is not found
        /// in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public bool Remove(KeyValuePair<string, object?> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Returns a subset of the current Dynamo object
        /// </summary>
        /// <param name="keys">Property keys to return</param>
        /// <returns>A new Dynamo object containing only the keys specified</returns>
        public dynamic SubSet(params string[] keys)
        {
            if (keys is null)
            {
                return new Dynamo();
            }

            var ReturnValue = new Dynamo();
            for (var i = 0; i < keys.Length; i++)
            {
                var Key = keys[i];
                ReturnValue.Add(Key, this[Key]);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Converts the object to the type specified
        /// </summary>
        /// <typeparam name="TObject">Object type</typeparam>
        /// <returns>The object converted to the type specified</returns>
        public TObject To<TObject>() => (TObject)To(typeof(TObject));

        /// <summary>
        /// Converts the object to the type specified
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <returns>The object converted to the type specified</returns>
        public object To(Type ObjectType)
        {
            var Result = AOPManager?.Create(ObjectType) ?? Activator.CreateInstance(ObjectType);
            DataMapper?.Map(GetType(), ObjectType)
                      .AutoMap()
                      .Copy(this, Result);
            return Result!;
        }

        /// <summary>
        /// Outputs the object graph
        /// </summary>
        /// <returns>The string version of the object</returns>
        public override string ToString()
        {
            var Builder = BuilderPool?.Get() ?? new StringBuilder();
            Builder.Append(GetType().Name).AppendLine(" this");
            foreach (var Key in Keys.OrderBy(x => x))
            {
                var Item = GetValue(Key, typeof(object));
                if (Item is null)
                {
                    Builder.Append("\tobject ").Append(Key).AppendLine(" = null");
                }
                else
                {
                    Builder.Append("\t").Append(Item.GetType().GetName()).Append(" ").Append(Key).Append(" = ").AppendLine(Item.ToString());
                }
            }
            var Result = Builder.ToString();
            BuilderPool?.Return(Builder);
            return Result;
        }

        /// <summary>
        /// Attempts to convert the object
        /// </summary>
        /// <param name="binder">Convert binder</param>
        /// <param name="result">Result</param>
        /// <returns>True if it is converted, false otherwise</returns>
        public override bool TryConvert(ConvertBinder binder, out object? result)
        {
            if (binder is null)
            {
                result = null;
                return false;
            }
            result = To(binder.Type);
            return true;
        }

        /// <summary>
        /// Attempts to get a member
        /// </summary>
        /// <param name="binder">GetMemberBinder object</param>
        /// <param name="result">Result</param>
        /// <returns>True if it gets the member, false otherwise</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            if (binder is null)
            {
                result = null;
                return false;
            }
            result = GetValue(binder.Name, binder.ReturnType);
            return true;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is
        /// found; otherwise, the default value for the type of the <paramref name="value"/>
        /// parameter. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the object that implements <see
        /// cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the
        /// specified key; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
        {
            value = GetValue(key, typeof(object));
            return true;
        }

        /// <summary>
        /// Attempts to set the member
        /// </summary>
        /// <param name="binder">Member binder</param>
        /// <param name="value">Value</param>
        /// <returns>True if it is set, false otherwise</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (binder is null)
                return false;
            TrySetValue(binder.Name, value);
            return true;
        }

        /// <summary>
        /// Tries to set the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if it is set, false otherwise.</returns>
        public bool TrySetValue(string key, object? value)
        {
            object? OldValue = null;
            var TempHashCode = HashCode ^ (key?.GetHashCode(StringComparison.OrdinalIgnoreCase) ?? EmptyHashCode);
            HashCode = TempHashCode ^ (value?.GetHashCode() ?? EmptyHashCode);
            if (TypeInfo?.TrySetValue(this, key, value, out OldValue) ?? false)
            {
                RaisePropertyChanged(key, OldValue, value);
                return true;
            }
            return SetInternalValue(key, value);
        }

        /// <summary>
        /// Writes the data to an XML doc
        /// </summary>
        /// <param name="writer">XML writer</param>
        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            if (writer is null)
                return;
            foreach (var Key in Keys)
            {
                writer.WriteElementString(Key, (string?)GetValue(Key, typeof(string)));
            }
        }

        /// <summary>
        /// Gets a value
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="returnType">Return value type</param>
        /// <returns>The returned value</returns>
        protected object? GetValue(string name, Type returnType)
        {
            var Value = RaiseGetValueStart(name);
            if (!(Value is null))
                return Value;

            if (GetInternalValue(name, out Value))
                return Value.To(returnType, null);

            object? Result = null;
            if (!(TypeInfo?.TryGetValue(this, name, out Result) ?? false))
                return ((object?)null)!.To<object>(returnType);

            var ReturnValue = Result.To(returnType, null);
            Value = RaiseGetValueEnd(name, ReturnValue);
            return Value ?? ReturnValue;
        }

        /// <summary>
        /// Raises the get value end event
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="value">Value initially being returned</param>
        /// <returns>
        /// Returns null if the function should continue, any other value should be immediately
        /// returned to the user
        /// </returns>
        protected object? RaiseGetValueEnd(string propertyName, object? value)
        {
            var End = new EventArgs.OnEndEventArgs { Content = value };
            getValueEnd_?.Invoke(this, propertyName, End);
            return End.Stop ? End.Content : null;
        }

        /// <summary>
        /// Raises the get value start event
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>
        /// Returns null if the function should continue, any other value should be immediately
        /// returned to the user
        /// </returns>
        protected object? RaiseGetValueStart(string propertyName)
        {
            var Start = new EventArgs.OnStartEventArgs { Content = propertyName };
            getValueStart_?.Invoke(this, Start);
            return Start.Stop ? Start.Content : null;
        }

        /// <summary>
        /// Raises the property changed event
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">New value for the property</param>
        protected void RaisePropertyChanged(string propertyName, object? oldValue, object? newValue)
        {
            if (ChangeLog != null)
            {
                var TempChange = new Change(oldValue, newValue);
                ChangeLog.AddOrUpdate(propertyName, TempChange, (_, __) => TempChange);
            }
            propertyChanged_?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets the internal value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if it is found, false otherwise.</returns>
        private bool GetInternalValue(string key, out object? value)
        {
            var TempData = Data;
            var Index = TempData.Definition.GetIndex(key);
            if (Index == -1)
            {
                value = null;
                return false;
            }
            var ReturnValue = TempData[Index];
            if (ReturnValue == UninitializedObject)
            {
                value = null;
                return false;
            }
            value = ReturnValue;
            return true;
        }

        /// <summary>
        /// Sets the internal value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private bool SetInternalValue(string key, object? value)
        {
            DynamoData TempData;
            object? OldValue;
            lock (LockObject)
            {
                TempData = Data;
                var Index = TempData.Definition.GetIndex(key);
                if (Index == -1)
                {
                    Data = Data.UpdateClass(TempData.Definition.AddKey(key));
                    TempData = Data;
                    Index = TempData.Definition.GetIndex(key);
                }
                OldValue = TempData[Index];
                if (OldValue == UninitializedObject)
                {
                    ++Count;
                }
                TempData[Index] = value;
            }
            if (value != OldValue)
                RaisePropertyChanged(key, OldValue, value);
            return true;
        }
    }
}