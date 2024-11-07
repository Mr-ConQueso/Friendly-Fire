using System;
using System.Collections.Generic;

public abstract class Pool<T> : IDisposable where T : Pool<T>, new()
{
    private static readonly Stack<Pool<T>> poolStack = new Stack<Pool<T>>();

    private bool _disposed;

    public static T Get()
    {
        T val = null;
        if (poolStack.Count > 0)
        {
            val = (T)poolStack.Pop();
        }
        if (val == null)
        {
            val = new T();
        }
        val._disposed = false;
        return val;
    }

    protected abstract void Deinitialize();

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
            Deinitialize();
            if (disposing)
            {
                poolStack.Push(this);
            }
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    ~Pool()
    {
        Dispose(disposing: false);
    }
}