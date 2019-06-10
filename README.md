![logo](https://raw.githubusercontent.com/Jac21/ShardedCounter.Core/master/media/logo.png)

🎰 Simplistic, atomic, interlocked counter that allows for huge numbers of operations to be performed using a "sharding" style approach to summation, all in .NET Core C#

## Interface
This library implements the following simplistic interface:

```csharp
    /// <summary>
    /// A simple counter interface to be used by the Sharded implementation
    /// </summary>
    internal interface ICounter
    {
        /// <summary>
        /// Increase the count by the amount provided
        /// </summary>
        /// <param name="amount">The amount to increase the counter</param>
        void Increase(long amount);

        /// <summary>
        /// Decrease the count by the amount provided
        /// </summary>
        /// <param name="amount">The amount to decrease the counter</param>
        void Decrease(long amount);

        /// <summary>
        /// Get the current count
        /// </summary>
        /// <returns>The current count</returns>
        long Count { get; }
    }
```
