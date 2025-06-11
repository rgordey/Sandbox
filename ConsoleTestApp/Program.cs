using BenchmarkDotNet.Running;
using Presentation.Benchmark;

class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<QueryBenchmark>();
    }
}
