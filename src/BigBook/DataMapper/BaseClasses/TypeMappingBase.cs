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

using BigBook.DataMapper.Interfaces;
using BigBook.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BigBook.DataMapper.BaseClasses
{
    /// <summary>
    /// Type mapping base class
    /// </summary>
    /// <typeparam name="TLeft">The type of the eft.</typeparam>
    /// <typeparam name="TRight">The type of the ight.</typeparam>
    /// <seealso cref="BigBook.DataMapper.Interfaces.ITypeMapping{Left, Right}"/>
    public abstract class TypeMappingBase<TLeft, TRight> : ITypeMapping<TLeft, TRight>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected TypeMappingBase()
        {
            Mappings = Array.Empty<IMapping<TLeft, TRight>>();
        }

        /// <summary>
        /// The current size
        /// </summary>
        protected int Count { get; set; }

        /// <summary>
        /// List of mappings
        /// </summary>
        protected IMapping<TLeft, TRight>[] Mappings { get; private set; }

        /// <summary>
        /// The underscore
        /// </summary>
        private const string Underscore = "_";

        /// <summary>
        /// The lock object
        /// </summary>
        private readonly object LockObject = new object();

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightExpression">Right expression</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<TLeft, TRight> AddMapping(Expression<Func<TLeft, object>> leftExpression, Expression<Func<TRight, object>> rightExpression);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set action</param>
        /// <param name="rightExpression">Right expression</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<TLeft, TRight> AddMapping(Func<TLeft, object> leftGet, Action<TLeft, object> leftSet, Expression<Func<TRight, object>> rightExpression);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<TLeft, TRight> AddMapping(Expression<Func<TLeft, object>> leftExpression, Func<TRight, object> rightGet, Action<TRight, object> rightSet);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set function</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<TLeft, TRight> AddMapping(Func<TLeft, object> leftGet, Action<TLeft, object> leftSet, Func<TRight, object> rightGet, Action<TRight, object> rightSet);

        /// <summary>
        /// Automatically maps properties that are named the same thing
        /// </summary>
        /// <returns>This</returns>
        public virtual ITypeMapping AutoMap()
        {
            if (Count > 0)
            {
                return this;
            }
            var RightDictionary = typeof(TRight).Is<IDictionary<string, object>>();
            var LeftDictionary = typeof(TLeft).Is<IDictionary<string, object>>();

            if (RightDictionary)
            {
                if (LeftDictionary)
                {
                    AddIDictionaryMappings();
                }
                else
                {
                    AddRightIDictionaryMapping();
                }
            }
            else if (LeftDictionary)
            {
                AddLeftIDictionaryMapping();
            }
            else
            {
                var Properties = TypeCacheFor<TLeft>.Properties;
                for (var x = 0; x < Properties.Length; ++x)
                {
                    var DestinationProperty = Array.Find(TypeCacheFor<TRight>.Properties, y => y.Name == Properties[x].Name);
                    if (DestinationProperty is null || (DestinationProperty.GetSetMethod()?.IsStatic ?? true))
                        continue;
                    var LeftGet = Properties[x].PropertyGetter<TLeft>();
                    var RightGet = DestinationProperty.PropertyGetter<TRight>();
                    AddMapping(LeftGet, RightGet);
                }
            }
            return this;
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public void Copy(object source, object destination) => Copy((TLeft)source, (TRight)destination);

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public abstract void Copy(TLeft source, TRight destination);

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public abstract void Copy(TRight source, TLeft destination);

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        public abstract void CopyLeftToRight(TLeft source, TRight destination);

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        public abstract void CopyRightToLeft(TRight source, TLeft destination);

        /// <summary>
        /// Creates the reversed.
        /// </summary>
        /// <returns>The type mapping</returns>
        public abstract ITypeMapping CreateReversed();

        /// <summary>
        /// Adds the mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        protected void AddMapping(IMapping<TLeft, TRight> mapping)
        {
            IMapping<TLeft, TRight>[] TempMappings;
            lock (LockObject)
            {
                TempMappings = Mappings;
                var OldLength = TempMappings.Length;
                ++Count;
                if (OldLength >= Count)
                {
                    TempMappings[Count - 1] = mapping;
                    Mappings = TempMappings;
                    return;
                }
                var NewData = new IMapping<TLeft, TRight>[GetAlignedSize(Count)];
                Array.Copy(TempMappings, NewData, OldLength);
                NewData[OldLength] = mapping;
                Mappings = NewData;
            }
        }

        /// <summary>
        /// Aligns the length to the value specified.
        /// </summary>
        /// <param name="len">The length.</param>
        /// <returns>The aligned size.</returns>
        private static int GetAlignedSize(int len)
        {
            const int DataAlignment = 8;
            return (len + (DataAlignment - 1)) & (~(DataAlignment - 1));
        }

        /// <summary>
        /// Rights the dictionary copy to left dictionary.
        /// </summary>
        /// <param name="right">The right.</param>
        /// <param name="left">The left.</param>
        private static void LeftDictionaryCopyToRightDictionary(TRight right, object left)
        {
            var LeftSide = (IDictionary<string, object>)left;
            var RightSide = (IDictionary<string, object>)right!;
            LeftSide.CopyTo(RightSide);
        }

        /// <summary>
        /// Rights the dictionary copy to left dictionary.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        private static void RightDictionaryCopyToLeftDictionary(TLeft left, object right)
        {
            var LeftSide = (IDictionary<string, object>)left!;
            var RightSide = (IDictionary<string, object>)right;
            RightSide.CopyTo(LeftSide);
        }

        /// <summary>
        /// Adds the i dictionary mappings.
        /// </summary>
        private void AddIDictionaryMappings()
        {
            AddMapping(x => x!,
            RightDictionaryCopyToLeftDictionary,
            x => x!,
            LeftDictionaryCopyToRightDictionary);
        }

        /// <summary>
        /// Adds the left i dictionary mapping.
        /// </summary>
        private void AddLeftIDictionaryMapping()
        {
            for (var x = 0; x < TypeCacheFor<TRight>.Properties.Length; ++x)
            {
                var Property = TypeCacheFor<TRight>.Properties[x];
                var RightGet = Property.PropertyGetter<TRight>();
                var LeftProperty = Array.Find(TypeCacheFor<TLeft>.Properties, y => y.Name == Property.Name);
                if (!(LeftProperty is null))
                {
                    var LeftGet = LeftProperty.PropertyGetter<TLeft>();
                    AddMapping(LeftGet, RightGet);
                }
                else
                {
                    var RightSet = RightGet.PropertySetter<TRight>()?.Compile();
                    AddMapping(new Func<TLeft, object>(y =>
                    {
                        var Temp = (IDictionary<string, object>)y!;
                        if (Temp.ContainsKey(Property.Name))
                        {
                            return Temp[Property.Name];
                        }

                        var Key = Temp.Keys.FirstOrDefault(z => string.Equals(z.Replace(TypeMappingBase<TLeft, TRight>.Underscore, string.Empty, StringComparison.Ordinal), Property.Name, StringComparison.OrdinalIgnoreCase));
                        return !string.IsNullOrEmpty(Key) ? Temp[Key] : null!;
                    }),
                    new Action<TLeft, object>((y, z) =>
                    {
                        var LeftSide = (IDictionary<string, object>)y!;
                        if (LeftSide.ContainsKey(Property.Name))
                        {
                            LeftSide[Property.Name] = z;
                        }
                        else
                        {
                            LeftSide.Add(Property.Name, z);
                        }
                    }),
                    (RightGet?.Compile())!,
                    new Action<TRight, object>((y, z) =>
                    {
                        if (z is null || RightSet is null)
                            return;
                        RightSet(y, z);
                    }));
                }
            }
        }

        /// <summary>
        /// Adds the right idictionary mapping.
        /// </summary>
        private void AddRightIDictionaryMapping()
        {
            for (var x = 0; x < TypeCacheFor<TLeft>.Properties.Length; ++x)
            {
                var Property = TypeCacheFor<TLeft>.Properties[x];
                var LeftGet = Property.PropertyGetter<TLeft>();
                var RightProperty = Array.Find(TypeCacheFor<TRight>.Properties, y => y.Name == Property.Name);
                if (!(RightProperty is null))
                {
                    var RightGet = RightProperty.PropertyGetter<TRight>();
                    AddMapping(LeftGet, RightGet);
                }
                else
                {
                    var LeftSet = LeftGet.PropertySetter<TLeft>()?.Compile();
                    AddMapping((LeftGet?.Compile())!,
                    new Action<TLeft, object>((y, z) =>
                    {
                        if (z is null || LeftSet is null)
                            return;
                        LeftSet(y, z);
                    }),
                    new Func<TRight, object>(y =>
                    {
                        var Temp = (IDictionary<string, object>)y!;
                        if (Temp.ContainsKey(Property.Name))
                        {
                            return Temp[Property.Name];
                        }

                        var Key = Temp.Keys.FirstOrDefault(z => string.Equals(z.Replace(TypeMappingBase<TLeft, TRight>.Underscore, string.Empty, StringComparison.Ordinal), Property.Name, StringComparison.OrdinalIgnoreCase));
                        return !string.IsNullOrEmpty(Key) ? Temp[Key] : null!;
                    }),
                    new Action<TRight, object>((y, z) =>
                    {
                        var LeftSide = (IDictionary<string, object>)y!;
                        if (LeftSide.ContainsKey(Property.Name))
                        {
                            LeftSide[Property.Name] = z;
                        }
                        else
                        {
                            LeftSide.Add(Property.Name, z);
                        }
                    }));
                }
            }
        }
    }
}