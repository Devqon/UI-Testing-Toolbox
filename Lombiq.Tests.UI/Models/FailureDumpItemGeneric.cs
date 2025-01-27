using System;
using System.IO;
using System.Threading.Tasks;

namespace Lombiq.Tests.UI.Models;

public class FailureDumpItemGeneric<TContent> : IFailureDumpItem
{
    private readonly TContent _content;
    private readonly Func<TContent, Task<Stream>> _getStream;
    private readonly Action<TContent> _dispose;
    private bool _disposed;

    public FailureDumpItemGeneric(
        TContent content,
        Func<TContent, Task<Stream>> getStream = null,
        Action<TContent> dispose = null)
    {
        if (content is not Stream && getStream == null)
        {
            throw new ArgumentException($"{nameof(content)} is not a Stream, {nameof(getStream)} can't be null");
        }

        _content = content;
        _getStream = getStream;
        _dispose = dispose;
    }

    public Task<Stream> GetStreamAsync()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(FailureDumpItemGeneric<TContent>));
        }

        if (_content is Stream stream && _getStream == null)
        {
            return Task.FromResult(stream);
        }

        return _getStream(_content);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_content is IDisposable disposable && _dispose == null)
                {
                    disposable.Dispose();
                }
                else
                {
                    _dispose?.Invoke(_content);
                }
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
