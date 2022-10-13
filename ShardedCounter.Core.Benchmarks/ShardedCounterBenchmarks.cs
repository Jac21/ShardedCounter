using BenchmarkDotNet.Attributes;

namespace ShardedCounter.Core.Benchmarks;

/// <summary>
/// ShardedCounter structure basic benchmark testing
/// </summary>
public class ShardedCounterBenchmarks
{
    [Benchmark]
    public void ShardedCounterIncreaseByOne()
    {
        // arrange
        var shardedCounter = new ShardedCounter();

        // act
        shardedCounter.Increase(1L);
    }

    [Benchmark]
    public void ShardedCounterIncreaseByOneHundredMillion()
    {
        // arrange
        var shardedCounter = new ShardedCounter();

        // act
        for (var i = 0; i < 100000000; i++)
        {
            shardedCounter.Increase(i);
        }
    }

    [Benchmark]
    public void ShardedCounterDecreaseByOne()
    {
        // arrange
        var shardedCounter = new ShardedCounter();

        // act
        shardedCounter.Decrease(-1L);
    }

    [Benchmark]
    public void ShardedCounterDecreaseByOneHundredMillion()
    {
        // arrange
        const long increasedCounterValue = 4999999950000000L;
        var shardedCounter = new ShardedCounter();

        // act
        shardedCounter.Increase(increasedCounterValue);

        for (var i = 0; i < 100000000; i++)
        {
            shardedCounter.Decrease(i);
        }
    }
}