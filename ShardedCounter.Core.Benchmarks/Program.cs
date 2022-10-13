using BenchmarkDotNet.Running;

namespace ShardedCounter.Core.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}