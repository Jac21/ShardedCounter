using System.Threading;

namespace ShardedCounter.Core.Structures
{
    /// <summary>
    /// Shard object representation, which is an InterlockedCounter 
    /// so that Count property sees the latest values of all the counters, 
    /// and thus avoids undercounting.
    /// </summary>
    internal class Shard : InterlockedCounter
    {
        public Thread Owner { get; set; }
    }
}