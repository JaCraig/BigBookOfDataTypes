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
using System.Collections.Generic;
using System.Linq;

namespace BigBook
{
    /// <summary>
    /// Binary tree
    /// </summary>
    /// <typeparam name="T">The type held by the nodes</typeparam>
    public class BinaryTree<T> : ICollection<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="root">Root of the binary tree</param>
        public BinaryTree(TreeNode<T> root = null)
        {
            if (Root == null)
            {
                NumberOfNodes = 0;
                return;
            }
            Root = root;
            NumberOfNodes = Traversal(Root).Count();
        }

        /// <summary>
        /// Number of items in the tree
        /// </summary>
        public int Count => NumberOfNodes;

        /// <summary>
        /// Is the tree empty
        /// </summary>
        public bool IsEmpty => Root == null;

        /// <summary>
        /// Is this read only?
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets the maximum value of the tree
        /// </summary>
        public T MaxValue
        {
            get
            {
                if (IsEmpty || Root == null)
                {
                    return default(T);
                }

                TreeNode<T> TempNode = Root;
                while (TempNode.Right != null)
                {
                    TempNode = TempNode.Right;
                }

                return TempNode.Value;
            }
        }

        /// <summary>
        /// Gets the minimum value of the tree
        /// </summary>
        public T MinValue
        {
            get
            {
                if (IsEmpty || Root == null)
                {
                    return default(T);
                }

                TreeNode<T> TempNode = Root;
                while (TempNode.Left != null)
                {
                    TempNode = TempNode.Left;
                }

                return TempNode.Value;
            }
        }

        /// <summary>
        /// The root value
        /// </summary>
        public TreeNode<T> Root { get; set; }

        /// <summary>
        /// The number of nodes in the tree
        /// </summary>
        protected int NumberOfNodes { get; set; }

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>The value as a string</returns>
        public static implicit operator string(BinaryTree<T> value)
        {
            if (value == null)
            {
                return "";
            }

            return value.ToString();
        }

        /// <summary>
        /// Adds an item to a binary tree
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(T item)
        {
            if (Root == null)
            {
                Root = new TreeNode<T>(item);
                ++NumberOfNodes;
            }
            else
            {
                Insert(item);
            }
        }

        /// <summary>
        /// Clears all items from the tree
        /// </summary>
        public void Clear()
        {
            Root = null;
            NumberOfNodes = 0;
        }

        /// <summary>
        /// Determines if the tree contains an item
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public bool Contains(T item)
        {
            if (IsEmpty)
            {
                return false;
            }

            TreeNode<T> TempNode = Root;
            while (TempNode != null)
            {
                var ComparedValue = TempNode.Value.CompareTo(item);
                if (ComparedValue == 0)
                {
                    return true;
                }

                TempNode = ComparedValue < 0 ? TempNode.Left : TempNode.Right;
            }
            return false;
        }

        /// <summary>
        /// Copies the tree to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            T[] TempArray = new T[NumberOfNodes];
            int Counter = 0;
            foreach (T Value in this)
            {
                TempArray[Counter] = Value;
                ++Counter;
            }
            Array.Copy(TempArray, 0, array, arrayIndex, NumberOfNodes);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (TreeNode<T> TempNode in Traversal(Root))
            {
                yield return TempNode.Value;
            }
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (TreeNode<T> TempNode in Traversal(Root))
            {
                yield return TempNode.Value;
            }
        }

        /// <summary>
        /// Removes an item from the tree
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(T item)
        {
            var Item = Find(item);
            if (Item == null)
            {
                return false;
            }

            --NumberOfNodes;
            var Values = new List<T>();
            foreach (TreeNode<T> TempNode in Traversal(Item.Left))
            {
                Values.Add(TempNode.Value);
            }

            foreach (TreeNode<T> TempNode in Traversal(Item.Right))
            {
                Values.Add(TempNode.Value);
            }

            if (Item.Parent != null)
            {
                if (Item.Parent.Left == Item)
                {
                    Item.Parent.Left = null;
                }
                else
                {
                    Item.Parent.Right = null;
                }

                Item.Parent = null;
            }
            else
            {
                Root = null;
            }
            for (int x = 0, ValuesCount = Values.Count; x < ValuesCount; x++)
            {
                T Value = Values[x];
                Add(Value);
            }

            return true;
        }

        /// <summary>
        /// Outputs the tree as a string
        /// </summary>
        /// <returns>The string representation of the tree</returns>
        public override string ToString()
        {
            return this.ToString(x => x.ToString(), " ");
        }

        /// <summary>
        /// Finds a specific object
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <returns>The node if it is found</returns>
        protected TreeNode<T> Find(T item)
        {
            foreach (TreeNode<T> Item in Traversal(Root))
            {
                if (Item.Value.Equals(item))
                {
                    return Item;
                }
            }

            return null;
        }

        /// <summary>
        /// Inserts a value
        /// </summary>
        /// <param name="item">item to insert</param>
        protected void Insert(T item)
        {
            if (Root == null)
            {
                return;
            }

            TreeNode<T> TempNode = Root;
            while (true)
            {
                var ComparedValue = TempNode.Value.CompareTo(item);
                if (ComparedValue > 0)
                {
                    if (TempNode.Left == null)
                    {
                        TempNode.Left = new TreeNode<T>(item, TempNode);
                        ++NumberOfNodes;
                        return;
                    }
                    TempNode = TempNode.Left;
                }
                else if (ComparedValue < 0)
                {
                    if (TempNode.Right == null)
                    {
                        TempNode.Right = new TreeNode<T>(item, TempNode);
                        ++NumberOfNodes;
                        return;
                    }
                    TempNode = TempNode.Right;
                }
                else
                {
                    TempNode = TempNode.Right;
                }
            }
        }

        /// <summary>
        /// Traverses the list
        /// </summary>
        /// <param name="node">The node to start the search from</param>
        /// <returns>The individual items from the tree</returns>
        protected IEnumerable<TreeNode<T>> Traversal(TreeNode<T> node)
        {
            if (node != null)
            {
                if (node.Left != null)
                {
                    foreach (TreeNode<T> LeftNode in Traversal(node.Left))
                    {
                        yield return LeftNode;
                    }
                }
                yield return node;
                if (node.Right != null)
                {
                    foreach (TreeNode<T> RightNode in Traversal(node.Right))
                    {
                        yield return RightNode;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Node class for the Binary tree
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    public class TreeNode<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value of the node</param>
        /// <param name="parent">Parent node</param>
        /// <param name="left">Left node</param>
        /// <param name="right">Right node</param>
        public TreeNode(T value = default(T), TreeNode<T> parent = null, TreeNode<T> left = null, TreeNode<T> right = null)
        {
            Value = value;
            Right = right;
            Left = left;
            Parent = parent;
        }

        /// <summary>
        /// Is this a leaf
        /// </summary>
        public bool IsLeaf => Left == null && Right == null;

        /// <summary>
        /// Is this the root
        /// </summary>
        public bool IsRoot => Parent == null;

        /// <summary>
        /// Left node
        /// </summary>
        public TreeNode<T> Left { get; set; }

        /// <summary>
        /// Parent node
        /// </summary>
        public TreeNode<T> Parent { get; set; }

        /// <summary>
        /// Right node
        /// </summary>
        public TreeNode<T> Right { get; set; }

        /// <summary>
        /// Value of the node
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Visited?
        /// </summary>
        internal bool Visited { get; set; }

        /// <summary>
        /// Returns the node as a string
        /// </summary>
        /// <returns>String representation of the node</returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}