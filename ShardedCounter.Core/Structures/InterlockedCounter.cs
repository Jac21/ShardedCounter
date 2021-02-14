using ShardedCounter.Core.Interfaces;
using System;
using System.Threading;

namespace ShardedCounter.Core.Structures
{
    internal class InterlockedCounter : ICounter
    {
        private long _count;

        public long Count => Interlocked.CompareExchange(ref _count, 0, 0);

        public void Increase(long amount)
        {
            Interlocked.Add(ref _count, amount);
        }

        public void Decrease(long amount)
        {
            Interlocked.Add(ref _count, -Math.Abs(amount));
        }
    }
}