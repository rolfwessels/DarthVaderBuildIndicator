using System;
using System.Threading;

namespace BuildIndicatron.App.Core.Task
{
    public class TaskCompletionSource<T>
    {
        private T _result;
        private Exception _exception;
        private ManualResetEvent manualResetEvent;
        private Action<Task<T>> _action;
        private Task<T> _task;
        private readonly object _locker = new object();

        public TaskCompletionSource()
        {
             manualResetEvent = new ManualResetEvent(false);
        }

        public void SetResult(T result)
        {   
            _result = result;
            CallSet();
            
        }

        public void TrySetException(Exception errorException)
        {
            _exception = errorException;
            CallSet();
        }

        private void CallSet()
        {
            lock (_locker)
            {
                IsSet = true;
                manualResetEvent.Set();
                if (_action != null) _action(_task);
            }
        }

        public ManualResetEvent ManualResetEvent
        {
            get { return manualResetEvent; }
        }

        public Task<T> Task {
            get { return _task ?? (_task = new Task<T>(this)); }
        }

        public T Result
        {
            get { return _result; }
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public bool IsSet { get; set; }

        public void ContinueWith(Action<Task<T>> action)
        {
            lock (_locker)
            {
                _action = action;
                if (IsSet) _action(_task);
            }
        }
    }
}