namespace NotinoHomeWork.Application.Services;

/// <summary>
/// Service which converts content into another form.
/// </summary>
/// <typeparam name="TWriter">Generic parameter, which specifies the type of writer, which will be used to write the transformed data.</typeparam>
public interface IConverterService<TWriter> where TWriter : class
{
	/// <summary>
	/// Accepts content in form of <see cref="StreamReader"/> which is transformed into different format and written via the <see cref="writer"/>.
	/// </summary>
	/// <param name="reader">Reader for reading the original content</param>
	/// <param name="writer">Writer for writing the converted content.</param>
	/// <param name="cancellationToken">Cancellation token</param>
	Task ConvertAsync(StreamReader reader, TWriter writer, CancellationToken cancellationToken);
}