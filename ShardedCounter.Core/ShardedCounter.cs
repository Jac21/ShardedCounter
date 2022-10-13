using ShardedCounter.Core.Interfaces;
using ShardedCounter.Core.Structures;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ShardedCounter.Core
{
    /// <summary>
    /// ShardedCounter allocates a new thread-local storage slot via Thread.AllocateDataSlot(). 
    /// This creates a place to store the counter shard for each thread. Operations include increasing and decreasing the
    /// counter by a given amount, as well as reading the value of said counter.
    /// </summary>
    public class ShardedCounter : ICounter
    {
        /// <summary>
        /// Protects deadShardSum and shards
        /// </summary>
        private readonly object _shardedCounterLock = new();

        /// <summary>
        /// The total sum from the shards from the threads which have terminated
        /// </summary>
        private long _deadShardSum;

        /// <summary>
        /// The list of shards
        /// </summary>
        private List<Shard> _shards = new();

        /// <summary>
        /// The thread-local slot where shards are stored
        /// </summary>
        private readonly LocalDataStoreSlot _slot = Thread.AllocateDataSlot();

        /// <summary>
        /// Increase counter for this thread
        /// </summary>
        /// <param name="amount"></param>
        public void Increase(long amount)
        {
            var counter = IncreaseCounter();

            counter.Increase(amount);
        }

        /// <summary>
        /// Decrease counter for this thread
        /// </summary>
        /// <param name="amount"></param>
        public void Decrease(long amount)
        {
            var counter = IncreaseCounter();

            counter.Decrease(amount);
        }

        /// <summary>
        /// Current count of the sharded counter, which involves summing 
        /// the counts in all the shards.
        /// </summary>
        public long Count
        {
            get
            {
                // sum over all the shards, and clean up dead shards at the same time
                var sum = _deadShardSum;

                var livingShards = new List<Shard>();

                lock (_shardedCounterLock)
                {
                    foreach (var shard in _shards)
                    {
                        sum += shard.Count;

                        if (shard.Owner.IsAlive)
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

        /// <summary>
        /// Increase counter for this particular thread
        /// </summary>
        /// <returns></returns>
        private Shard IncreaseCounter()
        {
            if (Thread.GetData(_slot) is not Shard counter)
            {
                counter = new Shard
                {
                    Owner = Thread.CurrentThread
                };

                Thread.SetData(_slot, counter);

                lock (_shardedCounterLock)
                {
                    _shards.Add(counter);
                }
            }

            return counter;
        }
    }
}