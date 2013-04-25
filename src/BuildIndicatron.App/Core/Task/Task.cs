using System;
using System.Collections;

namespace BuildIndicatron.App.Core.Task
{
    public class Task<T>
    {
        private readonly TaskCompletionSource<T> _taskCompletionSource;

        public Task(TaskCompletionSource<T> taskCompletionSource)
        {
            _taskCompletionSource = taskCompletionSource;
        }

        public void ContinueWith(Action<Task<T>> action)
        {
            _taskCompletionSource.ContinueWith(action);
        }

        public T Result
        {
            get { return _taskCompletionSource.Result; }
        }

        public Exception Exception
        {
            get { return _taskCompletionSource.Exception; }
        }

    }
}