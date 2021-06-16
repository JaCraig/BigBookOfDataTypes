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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BigBook
{
    /// <summary>
    /// Observable List class
    /// </summary>
    /// <typeparam name="T">Object type that the list holds</typeparam>
    public class ObservableList<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged, IList
    {
        /// <summary>
        /// Initializes a virtual instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        public ObservableList()
        {
            BaseList = new List<T>();
        }

        /// <summary>
        /// Initializes a virtual instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the virtual list can initially store.</param>
        public ObservableList(int capacity)
        {
            BaseList = new List<T>(capacity);
        }

        /// <summary>
        /// Initializes a virtual instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public ObservableList(IEnumerable<T> collection)
        {
            BaseList = new List<T>();
            BaseList.AddRange(collection);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        public int Count => BaseList.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </summary>
        public bool IsFixedSize { get; }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly { get; }

        /// <summary>
        /// Gets a value indicating whether access to the <see
        /// cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized { get; }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        public object? SyncRoot { get; }

        /// <summary>
        /// Gets or sets the base list.
        /// </summary>
        /// <value>The base list.</value>
        private List<T> BaseList { get; }

        /// <summary>
        /// The delegates_
        /// </summary>
        private readonly List<NotifyCollectionChangedEventHandler> CollectionChangedDelegates = new List<NotifyCollectionChangedEventHandler>();

        /// <summary>
        /// The property changed delegates
        /// </summary>
        private readonly List<PropertyChangedEventHandler> PropertyChangedDelegates = new List<PropertyChangedEventHandler>();

        /// <summary>
        /// The collection changed
        /// </summary>
        private NotifyCollectionChangedEventHandler? collectionChanged_;

        /// <summary>
        /// The property changed
        /// </summary>
        private PropertyChangedEventHandler? propertyChanged_;

        /// <summary>
        /// Gets or sets the <see cref="object"/> at the specified index.
        /// </summary>
        /// <value>The <see cref="object"/>.</value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        object IList.this[int index] { get => this[index]!; set => this[index] = (T)value; }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                if (BaseList.Count == 0)
                    return default;
                if (index < 0)
                    index = 0;
                if (index >= BaseList.Count)
                    index = BaseList.Count - 1;
                return BaseList[index];
            }

            set
            {
                if (BaseList.Count == 0)
                    return;
                if (index < 0)
                    index = 0;
                if (index >= BaseList.Count)
                    index = BaseList.Count - 1;
                NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, BaseList[index]));
                BaseList[index] = value;
            }
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                if (!(collectionChanged_ is null))
                {
                    collectionChanged_ -= value;
                }
                CollectionChangedDelegates.Remove(value);
                collectionChanged_ += value;
                CollectionChangedDelegates.Add(value);
            }
            remove
            {
                collectionChanged_ += value;
                CollectionChangedDelegates.Add(value);
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!(propertyChanged_ is null))
                {
                    propertyChanged_ -= value;
                }
                PropertyChangedDelegates.Remove(value);
                propertyChanged_ += value;
                PropertyChangedDelegates.Add(value);
            }
            remove
            {
                propertyChanged_ += value;
                PropertyChangedDelegates.Add(value);
            }
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to be added to the end of the list. The value can be null for reference types.
        /// </param>
        public virtual void Add(T item)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            NotifyPropertyChanged(nameof(Count));
            BaseList.Add(item);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The object to add to the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The position into which the new element was inserted, or -1 to indicate that the item
        /// was not inserted into the collection.
        /// </returns>
        public int Add(object value)
        {
            if (value is T ItemToAdd)
                Add(ItemToAdd);
            return Count;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public virtual void AddRange(IEnumerable<T> collection)
        {
            collection ??= Array.Empty<T>();
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection));
            NotifyPropertyChanged(nameof(Count));
            BaseList.AddRange(collection);
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        public virtual void Clear()
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            NotifyPropertyChanged(nameof(Count));
            BaseList.Clear();
        }

        /// <summary>
        /// Clears the delegates from the list.
        /// </summary>
        public void ClearDelegates()
        {
            PropertyChangedDelegates.ForEach(x => propertyChanged_ -= x);
            PropertyChangedDelegates.Clear();
            CollectionChangedDelegates.ForEach(x => collectionChanged_ -= x);
            CollectionChangedDelegates.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains
        /// a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>true if <paramref name="item"/> is found in the collection; otherwise, false.</returns>
        public bool Contains(T item) => BaseList.Contains(item);

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Object"/> is found in the <see
        /// cref="T:System.Collections.IList"/>; otherwise, false.
        /// </returns>
        public bool Contains(object value)
        {
            if (value is T Item)
                Contains(Item);
            return false;
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array is null || BaseList.Count == 0)
                return;
            if (arrayIndex < 0)
                arrayIndex = 0;
            if (arrayIndex >= array.Length)
                arrayIndex = array.Length - 1;
            BaseList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see
        /// cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements
        /// copied from <see cref="T:System.Collections.ICollection"/>. The <see
        /// cref="T:System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="index">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        public void CopyTo(Array array, int index) => CopyTo((T[])array, index);

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate
        /// through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator() => BaseList.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
        /// through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => BaseList.GetEnumerator();

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item) => BaseList.IndexOf(item);

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>The index of <paramref name="value"/> if found in the list; otherwise, -1.</returns>
        public int IndexOf(object value)
        {
            if (value is T Item)
                return IndexOf(Item);
            return -1;
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.Generic.List`1"/> at the
        /// specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        public virtual void Insert(int index, T item)
        {
            if (index < 0)
                index = 0;
            if (index >= BaseList.Count)
                index = BaseList.Count - 1;
            if (BaseList.Count == 0)
                index = 0;
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            NotifyPropertyChanged(nameof(Count));
            BaseList.Insert(index, item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="value"/> should be inserted.
        /// </param>
        /// <param name="value">The object to insert into the <see cref="T:System.Collections.IList"/>.</param>
        public void Insert(int index, object value)
        {
            if (value is T Item)
                Insert(index, Item);
        }

        /// <summary>
        /// Inserts the range.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="collection">The collection.</param>
        public virtual void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection is null)
                return;
            if (index < 0)
                index = 0;
            if (index >= BaseList.Count)
                index = BaseList.Count - 1;
            if (BaseList.Count == 0)
                index = 0;
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection.ToList(), index));
            NotifyPropertyChanged(nameof(Count));
            BaseList.InsertRange(index, collection);
        }

        /// <summary>
        /// Notifies the list that an item in the list has been modified.
        /// </summary>
        /// <param name="itemChanged">The item that was changed.</param>
        public void NotifyObjectChanged(object itemChanged)
        {
            if (collectionChanged_ is null)
                return;
            collectionChanged_.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, itemChanged, itemChanged));
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="T:System.Collections.Generic.List`1"/>. The
        /// value can be null for reference types.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> is successfully removed; otherwise, false. This method
        /// also returns false if <paramref name="item"/> was not found in the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </returns>
        public virtual bool Remove(T item)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            NotifyPropertyChanged(nameof(Count));
            return BaseList.Remove(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The object to remove from the <see cref="T:System.Collections.IList"/>.</param>
        public void Remove(object value)
        {
            if (value is T Item)
                Remove(Item);
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public virtual int RemoveAll(Predicate<T> match)
        {
            if (match is null)
                return 0;
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, this.Where(x => match(x))));
            NotifyPropertyChanged(nameof(Count));
            return BaseList.RemoveAll(match);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public virtual void RemoveAt(int index)
        {
            if (BaseList.Count == 0)
                return;
            if (index < 0)
                return;
            if (index >= BaseList.Count)
                return;
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, this[index], index));
            NotifyPropertyChanged(nameof(Count));
            BaseList.RemoveAt(index);
        }

        /// <summary>
        /// Removes a range of elements from the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public virtual void RemoveRange(int index, int count)
        {
            if (BaseList.Count == 0)
                return;
            if (index < 0)
                return;
            if (index >= BaseList.Count)
                return;
            if (index + count >= BaseList.Count)
                count = BaseList.Count - index;
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                                                                                    this.ElementsBetween(index, index + count),
                                                                                    index));
            NotifyPropertyChanged(nameof(Count));
            BaseList.RemoveRange(index, count);
        }

        /// <summary>
        /// Notifies the collection changed.
        /// </summary>
        /// <param name="args">
        /// The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.
        /// </param>
        protected void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (collectionChanged_ is null)
                return;
            collectionChanged_.Invoke(this, args);
        }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyChanged_ is null)
                return;
            propertyChanged_.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}