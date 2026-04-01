using NUnit.Framework;
using Shouldly;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ShardedCounter.Core.Unit.Tests;

/// <summary>
/// ShardedCounter structure basic operations testing
/// </summary>
[TestFixture]
public class ShardedCounterTest
{
    [Test]
    public void ShardedCounterAddSignedAmount()
    {
        var shardedCounter = new ShardedCounter();

        shardedCounter.Add(5L);
        shardedCounter.Add(-2L);

        shardedCounter.Count.ShouldBe(3L, "ShardedCounter did not apply signed deltas correctly.");
    }

    [Test]
    public void ShardedCounterSupportsIncrementAndDecrement()
    {
        var shardedCounter = new ShardedCounter();

        shardedCounter.Increment();
        shardedCounter.Increment();
        shardedCounter.Decrement();

        shardedCounter.Count.ShouldBe(1L, "ShardedCounter increment/decrement helpers returned an unexpected total.");
    }

    [Test]
    public void ShardedCounterIncreaseByOne()
    {
        // arrange
        var shardedCounter = new ShardedCounter();

        // act
        shardedCounter.Increase(1L);

        // assert
        shardedCounter.Count.ShouldBe(1L, "ShardedCounter did not increase by one.");
    }

    [Test]
    public void ShardedCounterIncreaseByOneHundredMillion()
    {
        // arrange
        const long increasedCounterValue = 4999999950000000L;
        var shardedCounter = new ShardedCounter();

        // act
        for (var i = 0; i < 100000000; i++)
        {
            shardedCounter.Increase(i);
        }

        // assert
        shardedCounter.Count.ShouldBe(increasedCounterValue,
            "ShardedCounter did not increase one hundred million times.");
    }

    [Test]
    public void ShardedCounterDecreaseByOne()
    {
        // arrange
        var shardedCounter = new ShardedCounter();

        // act
        shardedCounter.Decrease(-1L);

        // assert
        shardedCounter.Count.ShouldBe(-1L, "ShardedCounter did not decrease by one.");
    }

    [Test]
    public void ShardedCounterDecreaseByPositiveOne()
    {
        var shardedCounter = new ShardedCounter();

        shardedCounter.Decrease(1L);

        shardedCounter.Count.ShouldBe(-1L, "ShardedCounter did not normalize a positive decrease amount.");
    }

    [Test]
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

        // assert
        shardedCounter.Count.ShouldBe(0L, "ShardedCounter did not decrease one hundred million times.");
    }

    [Test]
    public async Task ShardedCounterSupportsConcurrentWriters()
    {
        const int workerCount = 8;
        const int iterationsPerWorker = 10_000;
        var shardedCounter = new ShardedCounter();

        var tasks = Enumerable.Range(0, workerCount)
            .Select(_ => Task.Run(() =>
            {
                for (var i = 0; i < iterationsPerWorker; i++)
                {
                    shardedCounter.Increment();
                }
            }))
            .ToArray();

        await Task.WhenAll(tasks);

        shardedCounter.Count.ShouldBe(workerCount * iterationsPerWorker,
            "ShardedCounter did not preserve the expected total under concurrent writers.");
    }

    [Test]
    public async Task ShardedCounterCountCanBeReadDuringConcurrentWrites()
    {
        const int workerCount = 4;
        const int iterationsPerWorker = 5_000;
        var shardedCounter = new ShardedCounter();
        var observedCounts = new ConcurrentBag<long>();

        var writerTasks = Enumerable.Range(0, workerCount)
            .Select(_ => Task.Run(() =>
            {
                for (var i = 0; i < iterationsPerWorker; i++)
                {
                    shardedCounter.Increment();
                }
            }))
            .ToArray();

        var readerTask = Task.Run(async () =>
        {
            while (writerTasks.Any(task => !task.IsCompleted))
            {
                observedCounts.Add(shardedCounter.Count);
                await Task.Yield();
            }

            observedCounts.Add(shardedCounter.Count);
        });

        await Task.WhenAll(writerTasks);
        await readerTask;

        observedCounts.ShouldNotBeEmpty("Expected to observe at least one count during concurrent writes.");
        observedCounts.ShouldAllBe(count => count >= 0 && count <= workerCount * iterationsPerWorker);
        shardedCounter.Count.ShouldBe(workerCount * iterationsPerWorker,
            "ShardedCounter did not converge to the expected total after concurrent reads and writes.");
    }
}
