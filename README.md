![logo](https://raw.githubusercontent.com/Jac21/ShardedCounter.Core/master/media/logo.png)

[![NuGet Status](https://img.shields.io/nuget/v/ShardedCounter.svg?style=flat)](https://www.nuget.org/packages/ShardedCounter/)
[![MIT License](https://img.shields.io/badge/license-MIT-green.svg?style=flat)](https://opensource.org/licenses/mit-license.php)
[![CI](https://github.com/Jac21/ShardedCounter/actions/workflows/ci.yml/badge.svg)](https://github.com/Jac21/ShardedCounter/actions/workflows/ci.yml)
[![Release](https://github.com/Jac21/ShardedCounter/actions/workflows/release.yml/badge.svg)](https://github.com/Jac21/ShardedCounter/actions/workflows/release.yml)

Simplistic, atomic, interlocked counter that spreads writes across per-thread shards so concurrent updates do not all contend on a single shared value.

## Why this exists

`ShardedCounter` is optimized for write-heavy concurrent scenarios. Each thread writes to its own shard, and reads compute the current total by summing the shard values.

This tradeoff is useful when:

- many threads are updating the counter frequently
- reads are less frequent than writes
- a single `Interlocked` value becomes a hotspot under contention

## Installation

```bash
dotnet add package ShardedCounter
```

## Usage

```csharp
using ShardedCounter.Core;

var counter = new ShardedCounter();

counter.Add(5);
counter.Add(-2);
counter.Increment();
counter.Decrement();

Console.WriteLine(counter.Count); // 3
```

## Public API

```csharp
public class ShardedCounter
{
    public void Add(long amount);
    public void Increase(long amount);
    public void Decrease(long amount);
    public void Increment();
    public void Decrement();
    public long Count { get; }
}
```

`Add(long amount)` is the recommended API because it makes signed counter updates explicit.
`Increase` and `Decrease` remain available as convenience and compatibility methods.

## Target frameworks

The library currently targets:

- `net6.0`
- `net8.0`

## Notes

- Writes are cheap because they stay on a thread-local shard.
- Reads are more expensive because they sum all known shards.
- This library is best suited to counters that are updated often and read occasionally.

## Validation

- Unit tests cover signed updates, compatibility APIs, and concurrent writer scenarios.
- The benchmark project compares `ShardedCounter` against a plain `Interlocked` counter under multiple thread counts.

Run the benchmarks with:

```bash
dotnet run -c Release --project ShardedCounter.Core.Benchmarks
```

BenchmarkDotNet will emit markdown and other result artifacts under `BenchmarkDotNet.Artifacts/`.
If you want benchmark snapshots in the repo, rerun the benchmark project intentionally and check in the generated markdown summary you want to preserve.

## Releases

- CI runs on pushes and pull requests through [`.github/workflows/ci.yml`](/Users/jeremycantu/Downloads/Development/Github/ShardedCounter/.github/workflows/ci.yml).
- NuGet publishing is gated on version tags through [`.github/workflows/release.yml`](/Users/jeremycantu/Downloads/Development/Github/ShardedCounter/.github/workflows/release.yml).
- Create a tag like `v6.0.2` after updating package metadata and [CHANGELOG.md](/Users/jeremycantu/Downloads/Development/Github/ShardedCounter/CHANGELOG.md) to publish the next release.
