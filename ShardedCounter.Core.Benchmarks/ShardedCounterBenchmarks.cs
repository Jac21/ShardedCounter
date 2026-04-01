using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using System.Threading;

namespace ShardedCounter.Core.Benchmarks;

[ShortRunJob]
[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
public class ShardedCounterBenchmarks
{
    private const int OperationsPerThread = 100_000;

    [Params(1, 4, 8)]
    public int ThreadCount { get; set; }

    [Benchmark(Baseline = true)]
    public long InterlockedCounterWriteHeavy()
    {
        var counter = new BaselineInterlockedCounter();

        Parallel.For(0, ThreadCount, _ =>
        {
            for (var i = 0; i < OperationsPerThread; i++)
            {
                counter.Increment();
            }
        });

        return counter.Count;
    }

    [Benchmark]
    public long ShardedCounterWriteHeavy()
    {
        var counter = new ShardedCounter();

        Parallel.For(0, ThreadCount, _ =>
        {
            for (var i = 0; i < OperationsPerThread; i++)
            {
                counter.Increment();
            }
        });

        return counter.Count;
    }
}

internal sealed class BaselineInterlockedCounter
{
    private long _count;

    public long Count => Interlocked.Read(ref _count);

    public void Increment()
    {
        Interlocked.Increment(ref _count);
    }
}
