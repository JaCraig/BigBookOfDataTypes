using Canister.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BigBook.Comparison.Module
{
    /// <summary>
    /// Comparison module
    /// </summary>
    /// <seealso cref="IModule"/>
    public class ComparisonModule : IModule
    {
        /// <summary>
        /// Order to run it in
        /// </summary>
        public int Order { get; } = 1;

        /// <summary>
        /// Loads the module
        /// </summary>
        /// <param name="bootstrapper">Bootstrapper to register with</param>
        public void Load(IBootstrapper bootstrapper)
        {
            if (bootstrapper == null)
                return;

            bootstrapper.Register(typeof(GenericComparer<>), ServiceLifetime.Singleton)
                .Register(typeof(GenericEqualityComparer<>), ServiceLifetime.Singleton);
        }
    }
}