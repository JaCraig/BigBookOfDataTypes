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

namespace BigBook
{
    /// <summary>
    /// Special case
    /// </summary>
    /// <seealso cref="Trie{Char, String}"/>
    public class StringTrie : Trie<char, string>
    {
        /// <summary>
        /// Adds the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        public StringTrie Add(params string[] values)
        {
            if (values is null)
                return this;
            for (int x = 0; x < values.Length; ++x)
            {
                base.Add(values[x], values[x]);
            }
            return this;
        }
    }

    /// <summary>
    /// Trie class
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <typeparam name="TReturn">The type of the return.</typeparam>
    public class Trie<TObject, TReturn>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Trie{TObject, TReturn}"/> class.
        /// </summary>
        public Trie()
        {
            Root = new Node<TObject, TReturn>();
        }

        /// <summary>
        /// The root
        /// </summary>
        /// <value>The root.</value>
        private Node<TObject, TReturn> Root { get; }

        /// <summary>
        /// Adds the specified "word".
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="returnValue">The return value.</param>
        public Trie<TObject, TReturn> Add(IEnumerable<TObject> word, TReturn returnValue)
        {
            if (word is null)
                return this;
            var node = Root;

            foreach (var character in word)
            {
                node = node[character]
                     ?? (Trie<TObject, TReturn>.Node<TObject, TReturn>)(node[character] = new Node<TObject, TReturn>(character, node));
            }
            node.Values.Add(returnValue);
            return this;
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        public Trie<TObject, TReturn> Build()
        {
            var queue = new Queue<Node<TObject, TReturn>>();
            queue.Enqueue(Root);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                foreach (var child in node)
                    queue.Enqueue(child);
                if (node == Root)
                {
                    Root.Fail = Root;
                    continue;
                }

                var fail = node.Parent.Fail;

                while (fail[node.Word] is null && fail != Root)
                    fail = fail.Fail;

                node.Fail = fail[node.Word] ?? Root;
                if (node.Fail == node)
                    node.Fail = Root;
            }
            return this;
        }

        /// <summary>
        /// Finds all added words in the text.
        /// </summary>
        /// <param name="text">The text to search in.</param>
        /// <returns>The values that were added for the found words.</returns>
        public IEnumerable<TReturn> FindAll(TObject[] text)
        {
            if (text is null)
                yield break;
            var node = Root;

            foreach (var c in text)
            {
                while (node[c] is null && node != Root)
                    node = node.Fail;

                node = node[c] ?? Root;

                for (var t = node; t != Root; t = t.Fail)
                {
                    for (var i = 0; i < t.Values.Count; i++)
                        yield return t.Values[i];
                }
            }
        }

        /// <summary>
        /// Finds all added words in the text.
        /// </summary>
        /// <param name="text">The text to search in.</param>
        /// <returns>The values that were added for the found words.</returns>
        public IEnumerable<TReturn> FindAll(List<TObject> text)
        {
            if (text is null)
                yield break;
            var node = Root;

            foreach (var c in text)
            {
                while (node[c] is null && node != Root)
                    node = node.Fail;

                node = node[c] ?? Root;

                for (var t = node; t != Root; t = t.Fail)
                {
                    for (var i = 0; i < t.Values.Count; i++)
                        yield return t.Values[i];
                }
            }
        }

        /// <summary>
        /// Finds all added words in the text.
        /// </summary>
        /// <param name="text">The text to search in.</param>
        /// <returns>The values that were added for the found words.</returns>
        public IEnumerable<TReturn> FindAll(Span<TObject> text)
        {
            if (text.IsEmpty)
                return Array.Empty<TReturn>();
            var ReturnValue = new List<TReturn>();
            var node = Root;
            for (int x = 0; x < text.Length; ++x)
            {
                var c = text[x];
                while (node[c] is null && node != Root)
                    node = node.Fail;

                node = node[c] ?? Root;

                for (var t = node; t != Root; t = t.Fail)
                {
                    for (var i = 0; i < t.Values.Count; i++)
                        ReturnValue.Add(t.Values[i]);
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Finds all added words in the text.
        /// </summary>
        /// <param name="text">The text to search in.</param>
        /// <returns>The values that were added for the found words.</returns>
        public IEnumerable<TReturn> FindAll(ReadOnlySpan<TObject> text)
        {
            if (text.IsEmpty)
                return Array.Empty<TReturn>();
            var ReturnValue = new List<TReturn>();
            var node = Root;
            for (int x = 0; x < text.Length; ++x)
            {
                var c = text[x];
                while (node[c] is null && node != Root)
                    node = node.Fail;

                node = node[c] ?? Root;

                for (var t = node; t != Root; t = t.Fail)
                {
                    for (var i = 0; i < t.Values.Count; i++)
                        ReturnValue.Add(t.Values[i]);
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Finds the first added word in the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// The first value that was found. The default value is returned if nothing is found.
        /// </returns>
        public TReturn FindAny(Span<TObject> text)
        {
            if (text.IsEmpty)
                return default!;
            var node = Root;

            for (int x = 0; x < text.Length; ++x)
            {
                var c = text[x];
                while (node[c] is null && node != Root)
                    node = node.Fail;

                node = node[c] ?? Root;

                for (var t = node; t != Root; t = t.Fail)
                {
                    if (t.Values.Count > 0)
                        return t.Values[0];
                }
            }
            return default!;
        }

        /// <summary>
        /// Finds the first added word in the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// The first value that was found. The default value is returned if nothing is found.
        /// </returns>
        public TReturn FindAny(ReadOnlySpan<TObject> text)
        {
            if (text.IsEmpty)
                return default!;
            var node = Root;

            for (int x = 0; x < text.Length; ++x)
            {
                var c = text[x];
                while (node[c] is null && node != Root)
                    node = node.Fail;

                node = node[c] ?? Root;

                for (var t = node; t != Root; t = t.Fail)
                {
                    if (t.Values.Count > 0)
                        return t.Values[0];
                }
            }
            return default!;
        }

        /// <summary>
        /// Finds the first added word in the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// The first value that was found. The default value is returned if nothing is found.
        /// </returns>
        public TReturn FindAny(List<TObject> text)
        {
            if (text is null)
                return default!;
            var node = Root;

            foreach (var c in text)
            {
                while (node[c] is null && node != Root)
                    node = node.Fail;

                node = node[c] ?? Root;

                for (var t = node; t != Root; t = t.Fail)
                {
                    if (t.Values.Count > 0)
                        return t.Values[0];
                }
            }
            return default!;
        }

        /// <summary>
        /// Finds the first added word in the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// The first value that was found. The default value is returned if nothing is found.
        /// </returns>
        public TReturn FindAny(TObject[] text)
        {
            if (text is null)
                return default!;
            var node = Root;

            foreach (var c in text)
            {
                while (node[c] is null && node != Root)
                    node = node.Fail;

                node = node[c] ?? Root;

                for (var t = node; t != Root; t = t.Fail)
                {
                    if (t.Values.Count > 0)
                        return t.Values[0];
                }
            }
            return default!;
        }

        /// <summary>
        /// Node used in the Trie
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <typeparam name="TNodeValue">The type of the node value.</typeparam>
        private class Node<TNode, TNodeValue> : IEnumerable<Node<TNode, TNodeValue>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Node{TNode, TNodeValue}"/> class.
            /// </summary>
            public Node()
            {
                Children = new Dictionary<TNode, Node<TNode, TNodeValue>>();
                Values = new List<TNodeValue>();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Node{TNode, TNodeValue}"/> class.
            /// </summary>
            /// <param name="word">The word.</param>
            /// <param name="parent">The parent.</param>
            public Node(TNode word, Node<TNode, TNodeValue> parent)
                : this()
            {
                Word = word;
                Parent = parent;
            }

            /// <summary>
            /// Gets the children.
            /// </summary>
            /// <value>The children.</value>
            public Dictionary<TNode, Node<TNode, TNodeValue>> Children { get; }

            /// <summary>
            /// Gets or sets the fail.
            /// </summary>
            /// <value>The fail.</value>
            public Node<TNode, TNodeValue>? Fail { get; set; }

            /// <summary>
            /// Gets the parent.
            /// </summary>
            /// <value>The parent.</value>
            public Node<TNode, TNodeValue>? Parent { get; }

            /// <summary>
            /// Gets the values.
            /// </summary>
            /// <value>The values.</value>
            public List<TNodeValue> Values { get; }

            /// <summary>
            /// Gets the word.
            /// </summary>
            /// <value>The word.</value>
            public TNode Word { get; }

            /// <summary>
            /// Gets or sets the <see cref="Node{TNode, TNodeValue}"/> with the specified node.
            /// </summary>
            /// <value>The <see cref="Node{TNode, TNodeValue}"/>.</value>
            /// <param name="node">The node.</param>
            /// <returns></returns>
            public Node<TNode, TNodeValue>? this[TNode node]
            {
                get { return Children.ContainsKey(node) ? Children[node] : null; }
                set { Children[node] = value!; }
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            public IEnumerator<Node<TNode, TNodeValue>> GetEnumerator()
            {
                return Children.Values.GetEnumerator();
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
            /// Converts to string.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                return Word?.ToString() ?? string.Empty;
            }
        }
    }
}