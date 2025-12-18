# C# Async/Await Mastery Showcase

A console application demonstrating deep understanding of asynchronous programming in C# using modern patterns and best practices.

## Async/Await Fundamentals & Task Basics

### Features Demonstrated
- **Async/Await syntax** – Proper use of `async Task` and `await`.
- **HttpClient** – Reusable instance with User-Agent header.
- **Basic async operations** – Single page download, sequential downloads, delays.
- **WhenAll** – Parallel execution of multiple downloads.
- **ValueTask** – Example of lightweight async return.
- **Fire-and-forget** – Demonstration (with caution note).
- **CPU-bound work** – Offloading to ThreadPool with `Task.Run`.

### Key Examples
- Single page download with error handling.
- Sequential vs parallel downloads (time difference visible).
- Custom delay and timeout patterns.
- Combination of I/O and CPU work.

## Parallel Async with WhenAll

- Sequential download of 100 delayed pages (~100 seconds).
- Parallel download using Task.WhenAll (~2-5 seconds).
- Concurrency limit with SemaphoreSlim (real-world best practice).

## ConfigureAwait & SynchronizationContext

- ConfigureAwait(true) vs ConfigureAwait(false)
- Context capture in UI apps (WinForms/WPF)
- Deadlock scenario demonstration and prevention
- Best practice: Use ConfigureAwait(false) in library code

## CancellationToken

- CancellationTokenSource and Token usage.
- Cancelable HttpClient operations.
- Cancelling Task.WhenAll group.
- Graceful cancellation with OperationCanceledException handling.

## Timeout & Retry

- Timeout with CancellationTokenSource(timeout)
- Simple retry loop for transient failures
- Combined timeout + retry for resilient downloads
- Real-world reliability pattern

## Project Structure
