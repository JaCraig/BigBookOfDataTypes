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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BigBook.DataMapper.BaseClasses
{
    /// <summary>
    /// Type mapping base class
    /// </summary>
    /// <typeparam name="Left">The type of the eft.</typeparam>
    /// <typeparam name="Right">The type of the ight.</typeparam>
    /// <seealso cref="BigBook.DataMapper.Interfaces.ITypeMapping{Left, Right}"/>
    public abstract class TypeMappingBase<Left, Right> : ITypeMapping<Left, Right>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected TypeMappingBase()
        {
            Mappings = new ConcurrentBag<IMapping<Left, Right>>();
        }

        /// <summary>
        /// List of mappings
        /// </summary>
        protected ConcurrentBag<IMapping<Left, Right>> Mappings { get; }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightExpression">Right expression</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> leftExpression, Expression<Func<Right, object>> rightExpression);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set action</param>
        /// <param name="rightExpression">Right expression</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Func<Left, object> leftGet, Action<Left, object> leftSet, Expression<Func<Right, object>> rightExpression);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> leftExpression, Func<Right, object> rightGet, Action<Right, object> rightSet);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set function</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Func<Left, object> leftGet, Action<Left, object> leftSet, Func<Right, object> rightGet, Action<Right, object> rightSet);

        /// <summary>
        /// Automatically maps properties that are named the same thing
        /// </summary>
        /// <returns>This</returns>
        public virtual ITypeMapping AutoMap()
        {
            if (Mappings.Count > 0)
            {
                return this;
            }
            var RightDictionary = typeof(Right).Is<IDictionary<string, object>>();
            var LeftDictionary = typeof(Left).Is<IDictionary<string, object>>();

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
                var Properties = TypeCacheFor<Left>.Properties;
                for (var x = 0; x < Properties.Length; ++x)
                {
                    var DestinationProperty = Array.Find(TypeCacheFor<Right>.Properties, y => y.Name == Properties[x].Name);
                    if (DestinationProperty == null || (DestinationProperty.GetSetMethod()?.IsStatic ?? true))
                        continue;
                    var LeftGet = Properties[x].PropertyGetter<Left>();
                    var RightGet = DestinationProperty.PropertyGetter<Right>();
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
        public void Copy(object source, object destination) => Copy((Left)source, (Right)destination);

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public abstract void Copy(Left source, Right destination);

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public abstract void Copy(Right source, Left destination);

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        public abstract void CopyLeftToRight(Left source, Right destination);

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        public abstract void CopyRightToLeft(Right source, Left destination);

        /// <summary>
        /// Creates the reversed.
        /// </summary>
        /// <returns>The type mapping</returns>
        public abstract ITypeMapping CreateReversed();

        private void AddIDictionaryMappings()
        {
            AddMapping(x => x!,
            new Action<Left, object>((x, y) =>
            {
                var LeftSide = (IDictionary<string, object>)x!;
                var RightSide = (IDictionary<string, object>)y;
                RightSide.CopyTo(LeftSide);
            }),
            x => x!,
            new Action<Right, object>((x, y) =>
            {
                var LeftSide = (IDictionary<string, object>)y;
                var RightSide = (IDictionary<string, object>)x!;
                LeftSide.CopyTo(RightSide);
            }));
        }

        private void AddLeftIDictionaryMapping()
        {
            for (var x = 0; x < TypeCacheFor<Right>.Properties.Length; ++x)
            {
                var Property = TypeCacheFor<Right>.Properties[x];
                var RightGet = Property.PropertyGetter<Right>();
                var LeftProperty = Array.Find(TypeCacheFor<Left>.Properties, y => y.Name == Property.Name);
                if (LeftProperty != null)
                {
                    var LeftGet = LeftProperty.PropertyGetter<Left>();
                    AddMapping(LeftGet, RightGet);
                }
                else
                {
                    var RightSet = RightGet.PropertySetter<Right>()?.Compile();
                    AddMapping(new Func<Left, object>(y =>
                    {
                        var Temp = (IDictionary<string, object>)y!;
                        if (Temp.ContainsKey(Property.Name))
                        {
                            return Temp[Property.Name];
                        }

                        var Key = Temp.Keys.FirstOrDefault(z => string.Equals(z.Replace("_", ""), Property.Name, StringComparison.OrdinalIgnoreCase));
                        if (!string.IsNullOrEmpty(Key))
                        {
                            return Temp[Key];
                        }

                        return null!;
                    }),
                    new Action<Left, object>((y, z) =>
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
                    RightGet?.Compile()!,
                    new Action<Right, object>((y, z) =>
                    {
                        if (z != null && RightSet != null)
                        {
                            RightSet(y, z);
                        }
                    }));
                }
            }
        }

        /// <summary>
        /// Adds the right idictionary mapping.
        /// </summary>
        private void AddRightIDictionaryMapping()
        {
            for (var x = 0; x < TypeCacheFor<Left>.Properties.Length; ++x)
            {
                var Property = TypeCacheFor<Left>.Properties[x];
                var LeftGet = Property.PropertyGetter<Left>();
                var RightProperty = Array.Find(TypeCacheFor<Right>.Properties, y => y.Name == Property.Name);
                if (RightProperty != null)
                {
                    var RightGet = RightProperty.PropertyGetter<Right>();
                    AddMapping(LeftGet, RightGet);
                }
                else
                {
                    var LeftSet = LeftGet.PropertySetter<Left>()?.Compile();
                    AddMapping(LeftGet?.Compile()!,
                    new Action<Left, object>((y, z) =>
                    {
                        if (z != null && LeftSet != null)
                        {
                            LeftSet(y, z);
                        }
                    }),
                    new Func<Right, object>(y =>
                    {
                        var Temp = (IDictionary<string, object>)y!;
                        if (Temp.ContainsKey(Property.Name))
                        {
                            return Temp[Property.Name];
                        }

                        var Key = Temp.Keys.FirstOrDefault(z => string.Equals(z.Replace("_", ""), Property.Name, StringComparison.OrdinalIgnoreCase));
                        if (!string.IsNullOrEmpty(Key))
                        {
                            return Temp[Key];
                        }

                        return null!;
                    }),
                    new Action<Right, object>((y, z) =>
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