![logo](https://raw.githubusercontent.com/Jac21/ShardedCounter.Core/master/media/logo.png)

[![NuGet Status](http://img.shields.io/nuget/v/ShardedCounter.svg?style=flat)](https://www.nuget.org/packages/ShardedCounter/)
[![MIT Licence](https://badges.frapsoft.com/os/mit/mit.svg?v=103)](https://opensource.org/licenses/mit-license.php)
[![Build Status](https://travis-ci.org/Jac21/ShardedCounter.Core.svg?branch=master)](https://travis-ci.org/Jac21/ShardedCounter.Core)
[![donate](https://img.shields.io/badge/%24-Buy%20me%20a%20coffee-ff69b4.svg?style=flat)](https://www.buymeacoffee.com/jac21)

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
