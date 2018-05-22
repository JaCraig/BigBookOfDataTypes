﻿using BenchmarkDotNet.Running;

namespace BigBook.Benchmarks
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            new BenchmarkSwitcher(typeof(Program).Assembly).Run(args);
        }
    }
}