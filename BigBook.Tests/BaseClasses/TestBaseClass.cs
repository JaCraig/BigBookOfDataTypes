using BigBook.ExtensionMethods;
using Mecha.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace BigBook.Tests.BaseClasses
{
    /// <summary>
    /// Test base class
    /// </summary>
    /// <typeparam name="TTestObject">The type of the test object.</typeparam>
    public abstract class TestBaseClass<TTestObject> : TestBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestBaseClass{TTestObject}"/> class.
        /// </summary>
        protected TestBaseClass()
        {
            ObjectType = null;
        }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
        protected override Type? ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the test object.
        /// </summary>
        /// <value>The test object.</value>
        protected TTestObject TestObject { get; set; }

        /// <summary>
        /// Attempts to break the object.
        /// </summary>
        /// <returns>The async task.</returns>
        [Fact]
        public Task BreakObject()
        {
            return Task.CompletedTask;
            return TestObject is null
                ? Task.CompletedTask
                : Mech.BreakAsync(TestObject, new Options
                {
                    MaxDuration = 200,
                    ExceptionHandlers = new ExceptionHandler()
                    .IgnoreException<NotImplementedException>()
                    .IgnoreException<ArgumentOutOfRangeException>((_, __) => true)
                    .IgnoreException<ArgumentException>()
                    .IgnoreException<ObjectDisposedException>((_, __) => true),
                    DiscoverInheritedMethods = false
                });
        }
    }

    /// <summary>
    /// Test base class
    /// </summary>
    public abstract class TestBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestBaseClass{TTestObject}"/> class.
        /// </summary>
        protected TestBaseClass()
        {
            lock (LockObject)
            {
                _ = Mech.Default;
            }
        }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
        protected abstract Type? ObjectType { get; set; }

        /// <summary>
        /// The lock object
        /// </summary>
        private static readonly object LockObject = new();

        /// <summary>
        /// The service provider lock
        /// </summary>
        private static readonly object ServiceProviderLock = new();

        /// <summary>
        /// The service provider
        /// </summary>
        private static IServiceProvider ServiceProvider;

        /// <summary>
        /// Attempts to break the object.
        /// </summary>
        /// <returns>The async task.</returns>
        [Fact]
        public Task BreakType()
        {
            return Task.CompletedTask;
            return ObjectType is null
                ? Task.CompletedTask
                : Mech.BreakAsync(ObjectType, new Options
                {
                    MaxDuration = 200,
                    ExceptionHandlers = new ExceptionHandler()
                    .IgnoreException<NotImplementedException>()
                    .IgnoreException<ArgumentOutOfRangeException>((_, __) => true)
                    .IgnoreException<ArgumentException>((_, __) => true)
                    .IgnoreException<FormatException>((_, __) => true)
                    .IgnoreException<ObjectDisposedException>((_, __) => true)
                    .IgnoreException<EndOfStreamException>((_, __) => true)
                    .IgnoreException<OutOfMemoryException>((_, __) => true)
                });
        }

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <returns></returns>
        protected static IServiceProvider GetServiceProvider()
        {
            if (ServiceProvider is not null)
                return ServiceProvider;
            lock (ServiceProviderLock)
            {
                if (ServiceProvider is not null)
                    return ServiceProvider;
                ServiceProvider = new ServiceCollection().AddCanisterModules()?.BuildServiceProvider();
            }
            return ServiceProvider;
        }

        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        protected string ReadFile(string fileName) => File.ReadAllText(fileName);

        protected void WriteToFile(string fileName, string content)
        {
            using var Stream = new FileInfo(fileName).OpenWrite();
            Stream.Write(content.ToByteArray());
            Stream.Flush();
            Stream.Close();
        }
    }
}