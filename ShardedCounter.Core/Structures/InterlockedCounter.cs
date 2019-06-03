using ShardedCounter.Core.Interfaces;
using System;
using System.Threading;

namespace ShardedCounter.Core.Structures
{
    internal class InterlockedCounter : ICounter
    {
        private long count;

        public long Count => Interlocked.CompareExchange(ref count, 0, 0);

        public void Increase(long amount)
        {
            Interlocked.Add(ref count, amount);
        }

        public void Decrease(long amount)
        {
            Interlocked.Add(ref count, -Math.Abs(amount));
        }
    }
}