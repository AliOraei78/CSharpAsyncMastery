using System.Runtime.CompilerServices;
using System.Threading;

public static class AsyncStreamDemo
{
    public static async IAsyncEnumerable<int> GenerateNumebrsAsync(
        int count = 100,
        int delayMs = 200,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        for (int i = 0; i <= count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Delay(delayMs, cancellationToken);

            Console.WriteLine($"Created: {i}");
            yield return i;
        }
    }
}