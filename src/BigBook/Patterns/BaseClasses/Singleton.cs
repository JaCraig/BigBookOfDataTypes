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
using System.Linq;
using System.Reflection;

namespace BigBook.Patterns.BaseClasses
{
    /// <summary>
    /// Base class used for singletons
    /// </summary>
    /// <typeparam name="T">The class type</typeparam>
    public class Singleton<T>
        where T : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected Singleton()
        {
        }

        /// <summary>
        /// Gets the instance of the singleton
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (Temp)
                    {
                        if (_Instance == null)
                        {
                            var Constructor = typeof(T).GetTypeInfo()
                                                                   .DeclaredConstructors
                                                                   .FirstOrDefault(x => !x.IsPublic
                                                                                     && !x.IsStatic
                                                                                     && x.GetParameters().Length == 0);
                            if (Constructor == null || Constructor.IsAssembly)
                                throw new InvalidOperationException("Constructor is not private or protected for type " + typeof(T).Name);
                            _Instance = (T)Constructor.Invoke(null);
                        }
                    }
                }
                return _Instance;
            }
        }

        private static T _Instance;

        private static object Temp = 1;
    }
}