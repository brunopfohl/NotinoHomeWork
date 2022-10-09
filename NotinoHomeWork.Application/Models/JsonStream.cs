namespace NotinoHomeWork;

/// <summary>
/// Represents <see cref="Stream"/> of JSON content.
/// </summary>
public class JsonStream : IDisposable
{
	private readonly Stream _stream;

	public JsonStream(Stream stream)
	{
		_stream = stream;
	}

	public void Dispose()
	{
		_stream.Dispose();
	}

	// Implicit operators
	public static implicit operator Stream(JsonStream o) => o._stream;
	public static implicit operator JsonStream(Stream s) => new(s);
}