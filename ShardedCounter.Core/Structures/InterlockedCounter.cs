using ShardedCounter.Core.Interfaces;
using System;
using System.Threading;

namespace ShardedCounter.Core.Structures;

internal class InterlockedCounter : ICounter
{
    private long _count;

    public long Count => Interlocked.Read(ref _count);

    public void Add(long amount)
    {
        Interlocked.Add(ref _count, amount);
    }

    public void Increase(long amount)
    {
        Add(amount);
    }

    public void Decrease(long amount)
    {
        if (amount == long.MinValue)
        {
            Add(long.MinValue);
            return;
        }

        Add(-Math.Abs(amount));
    }
}
