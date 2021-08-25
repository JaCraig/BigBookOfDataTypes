using BenchmarkDotNet.Attributes;
using System;
using System.Reflection.Emit;

namespace BigBook.Benchmarks.Tests
{
    [RankColumn, MemoryDiagnoser]
    public class ActivatorCreateInstanceTests
    {
        private Func<object>? CachedMethod;

        [Benchmark(Baseline = true)]
        public void ActivatorCreateInstance()
        {
            _ = Activator.CreateInstance(typeof(TestClass));
        }

        [Benchmark]
        public void CachedDynamicMethod()
        {
            _ = CachedMethod?.Invoke();
        }

        [Benchmark]
        public void DynamicMethod()
        {
            var type = typeof(TestClass);
            var target = type.GetConstructor(Type.EmptyTypes);
            var dynamic = new DynamicMethod(string.Empty,
                          type,
                          Array.Empty<Type>(),
                          target?.DeclaringType!);
            var il = dynamic.GetILGenerator();
            il.DeclareLocal(target?.DeclaringType!);
            il.Emit(OpCodes.Newobj, target!);
            //il.Emit(OpCodes.Stloc_0);
            //il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            var method = (Func<object>)dynamic.CreateDelegate(typeof(Func<object>));
            _ = method();
        }

        [GlobalSetup]
        public void Setup()
        {
            var type = typeof(TestClass);
            var target = type.GetConstructor(Type.EmptyTypes);
            var dynamic = new DynamicMethod(string.Empty,
                          type,
                          Array.Empty<Type>(),
                          target?.DeclaringType!);
            var il = dynamic.GetILGenerator();
            il.DeclareLocal(target?.DeclaringType!);
            il.Emit(OpCodes.Newobj, target!);
            //il.Emit(OpCodes.Stloc_0);
            //il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            CachedMethod = (Func<object>)dynamic.CreateDelegate(typeof(Func<object>));
        }

        private class TestClass
        {
            public string? A { get; set; }

            public int B { get; set; }

            public float C { get; set; }
        }
    }
}