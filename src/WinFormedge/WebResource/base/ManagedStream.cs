namespace WinFormedge.WebResource;
// https://learn.microsoft.com/zh-cn/microsoft-edge/webview2/concepts/working-with-local-content?tabs=dotnetcsharp
class ManagedStream : Stream
{
    public ManagedStream(Stream s)
    {
        _stream = s;
    }

    public override bool CanRead => _stream.CanRead;

    public override bool CanSeek => _stream.CanSeek;

    public override bool CanWrite => _stream.CanWrite;

    public override long Length => _stream.Length;

    public override long Position { get => _stream.Position; set => _stream.Position = value; }

    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _stream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int read = 0;
        try
        {
            read = _stream.Read(buffer, offset, count);
            if (read == 0)
            {
                _stream.Dispose();
            }
        }
        catch
        {
            _stream.Dispose();
            throw;
        }
        return read;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    private Stream _stream;
}
