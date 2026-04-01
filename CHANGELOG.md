# Changelog

All notable changes to this project should be documented in this file.

The format is based on Keep a Changelog, and versions should map to published NuGet package versions when possible.

## [Unreleased]

### Added
- GitHub Actions CI workflow for restore, build, test, and pack validation.
- GitHub Actions release workflow for tag-based NuGet publishing.
- `Add(long amount)`, `Increment()`, and `Decrement()` APIs.
- Concurrent correctness tests and comparative benchmarks against a plain `Interlocked` counter.

### Changed
- Modernized package metadata, Source Link support, symbol package generation, and target framework coverage for `net6.0` and `net8.0`.
- Replaced legacy thread-local storage APIs with `ThreadLocal<T>` in the counter implementation.
- Updated the README to better explain the library tradeoffs and consumer-facing API.

## [6.0.1]

### Changed
- Previous published release.
