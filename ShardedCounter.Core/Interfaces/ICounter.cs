namespace ShardedCounter.Core.Interfaces;

/// <summary>
/// A simple counter contract used by sharded counter implementations.
/// </summary>
internal interface ICounter
{
    /// <summary>
    /// Adds a signed amount to the counter.
    /// </summary>
    /// <param name="amount">The signed amount to apply.</param>
    void Add(long amount);

    /// <summary>
    /// Increases the counter by the amount provided.
    /// </summary>
    /// <param name="amount">The amount to increase the counter.</param>
    void Increase(long amount);

    /// <summary>
    /// Decreases the counter by the amount provided.
    /// </summary>
    /// <param name="amount">The amount to decrease the counter.</param>
    void Decrease(long amount);

    /// <summary>
    /// Gets the current count.
    /// </summary>
    long Count { get; }
}
