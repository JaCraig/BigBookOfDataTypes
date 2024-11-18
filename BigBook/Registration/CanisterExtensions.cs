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

using Aspectus.ExtensionMethods;
using BigBook.Comparison;
using BigBook.DynamoUtils;
using Canister.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using ObjectCartographer.ExtensionMethods;

namespace BigBook.Registration
{
    /// <summary>
    /// Canister registration extension
    /// </summary>
    public static class BigBookCanisterExtensions
    {
        /// <summary>
        /// Registers the big book of data types.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>The bootstrapper</returns>
        public static ICanisterConfiguration? RegisterBigBookOfDataTypes(this ICanisterConfiguration? bootstrapper)
        {
            return bootstrapper?.AddAssembly(typeof(BigBookCanisterExtensions).Assembly)
                               .RegisterAspectus()
                               .RegisterObjectCartographer();
        }

        /// <summary>
        /// Registers the big book of data types with the specified service collection.
        /// </summary>
        /// <param name="services">The service collection to register the data types with.</param>
        /// <returns>The service collection with the registered data types.</returns>
        public static IServiceCollection? RegisterBigBookOfDataTypes(this IServiceCollection? services)
        {
            var ObjectPoolProvider = new DefaultObjectPoolProvider();
            return services?.AddSingleton(typeof(GenericComparer<>))
                         .AddSingleton(typeof(GenericEqualityComparer<>))
                         .AddSingleton(new DynamoTypes())
                         .AddSingleton(ObjectPoolProvider.CreateStringBuilderPool())
                         .RegisterAspectus()
                         .RegisterObjectCartographer();
        }
    }
}