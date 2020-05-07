using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccountingNotebook.Utils
{
    public static class SemaphoreSlimExtension
    {
        public static async Task RunAsync(
            this System.Threading.SemaphoreSlim semaphore,
            Func<Task> action,
            System.Threading.CancellationToken cancellationToken)
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
            catch (ObjectDisposedException ex)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
            }
        }
    }
}
