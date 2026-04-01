using System.Threading;

namespace ShardedCounter.Core.Structures;

/// <summary>
/// A per-thread shard backed by interlocked operations.
/// </summary>
internal sealed class Shard : InterlockedCounter
{
    private readonly WeakReference<Thread> _owner;

    public Shard(Thread owner)
    {
        _owner = new WeakReference<Thread>(owner);
    }

    public bool IsOwnerAlive => _owner.TryGetTarget(out var owner) && owner.IsAlive;
}
