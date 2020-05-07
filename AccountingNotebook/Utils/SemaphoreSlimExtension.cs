using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccountingNotebook.Utils
{
    public static class SemaphoreSlimExtension
    {
        public static async Task RunAsync(
            this SemaphoreSlim semaphore,
            Func<Task> action,
            CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    await semaphore.WaitAsync(cancellationToken);
                    await action();
                }
                finally
                {
                    semaphore.Release();
                }
            }
            catch (OperationCanceledException ex)
            {
                if (cancellationToken != ex.CancellationToken || !cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
            }
            catch (ObjectDisposedException)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
            }
        }
    }
}
