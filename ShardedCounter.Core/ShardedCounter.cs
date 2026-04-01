using ShardedCounter.Core.Interfaces;
using ShardedCounter.Core.Structures;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ShardedCounter.Core;

/// <summary>
/// A contention-friendly counter that spreads writes across thread-local shards
/// and computes the current value by summing those shards when read.
/// </summary>
public class ShardedCounter : ICounter
{
    private readonly object _shardedCounterLock = new();
    private readonly ThreadLocal<Shard?> _threadShard = new();
    private long _deadShardSum;
    private List<Shard> _shards = new();

    /// <summary>
    /// Adds a signed delta to the current counter value.
    /// Positive values increase the counter and negative values decrease it.
    /// </summary>
    /// <param name="amount">The signed amount to apply to the counter.</param>
    public void Add(long amount)
    {
        if (amount == 0)
        {
            return;
        }

        var counter = GetOrCreateShard();

        if (amount > 0)
        {
            counter.Increase(amount);
            return;
        }

        counter.Decrease(amount);
    }

    /// <summary>
    /// Increases the counter by the supplied amount.
    /// </summary>
    /// <param name="amount">The amount to increase the counter by.</param>
    public void Increase(long amount)
    {
        Add(amount);
    }

    /// <summary>
    /// Decreases the counter by the supplied amount.
    /// Positive or negative values both reduce the counter.
    /// </summary>
    /// <param name="amount">The amount to decrease the counter by.</param>
    public void Decrease(long amount)
    {
        if (amount == 0)
        {
            return;
        }

        if (amount == long.MinValue)
        {
            Add(long.MinValue);
            return;
        }

        Add(-Math.Abs(amount));
    }

    /// <summary>
    /// Increments the counter by one.
    /// </summary>
    public void Increment()
    {
        Add(1);
    }

    /// <summary>
    /// Decrements the counter by one.
    /// </summary>
    public void Decrement()
    {
        Add(-1);
    }

    /// <summary>
    /// Gets the current total across all live and retired shards.
    /// </summary>
    public long Count
    {
        get
        {
            var sum = _deadShardSum;
            var livingShards = new List<Shard>();

            lock (_shardedCounterLock)
            {
                foreach (var shard in _shards)
                {
                    sum += shard.Count;

                    if (shard.IsOwnerAlive)
                    {
                        livingShards.Add(shard);
                    }
                    else
                    {
                        _deadShardSum += shard.Count;
                    }
                }

                _shards = livingShards;
            }

            return sum;
        }
    }

    private Shard GetOrCreateShard()
    {
        if (_threadShard.Value is { } counter)
        {
            return counter;
        }

        counter = new Shard(Thread.CurrentThread);
        _threadShard.Value = counter;

        lock (_shardedCounterLock)
        {
            _shards.Add(counter);
        }

        return counter;
    }
}
