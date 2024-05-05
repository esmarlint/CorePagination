using BenchmarkDotNet.Running;

namespace CorePagination.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var paginatorExtensionsSummary = BenchmarkRunner.Run<PaginatorExtensionsBenchmarks>();
            var paginatorsSummary = BenchmarkRunner.Run<PaginatorBenchmarks>();
        }
    }
}