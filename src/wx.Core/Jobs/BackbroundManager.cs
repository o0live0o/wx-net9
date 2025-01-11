using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace wx.Core.Jobs
{
    public class BackbroundManager<T> : IBackgroundManager
    {
        private readonly SemaphoreSlim _semaphore;

        private readonly BlockingCollection<T> _queue;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private Task _task;

        public BackbroundManager()
        {
            _semaphore = new SemaphoreSlim(1);
            _queue = new BlockingCollection<T>();
        }

        public void Add(T item)
        {
            _queue.Add(item);
        }

        private async Task Runner()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_task == null || _task.IsCompleted)
                {
                    _task = Task.Run(() =>
                    {
                        while (!_cts.IsCancellationRequested)
                        {
                            try
                            {
                                var item = _queue.Take(_cts.Token);

                            }
                            catch (OperationCanceledException) when (_queue.IsCompleted)
                            {
                                break;
                            }
                        }
                    });
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
