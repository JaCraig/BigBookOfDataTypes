using BenchmarkDotNet.Running;

namespace BigBook.Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new BenchmarkSwitcher(typeof(Program).Assembly).Run(args);
        }
    }
}