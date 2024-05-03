using BenchmarkDotNet.Running;

namespace CorePagination.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PaginatorBenchmarks>();
        }
    }
}