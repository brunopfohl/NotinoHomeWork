using Microsoft.AspNetCore.Mvc.Formatters;
using NotinoHomeWork.Application.Services;

namespace NotinoHomeWork.API.MediaFormatters;

/// <summary>
/// Base class for OutputFormatter from JSON to any other type.
/// OutputFormatters based on this class only react to content of type <see cref="JsonStream"/>.
/// </summary>
/// <typeparam name="TWriter">Type of writer, which is used to write to response stream.</typeparam>
/// <typeparam name="TConverter">Converter, which is used to convert data to another format.</typeparam>
public abstract class FromJsonOutputFormatterBase<TWriter, TConverter> : OutputFormatter
	where TConverter : IConverterService<TWriter>
	where TWriter : class

{
	/// <summary>
	/// Gets the content type of output.
	/// </summary>
	protected abstract string ContentType { get; }

	protected FromJsonOutputFormatterBase()
	{
		SupportedMediaTypes.Add(ContentType);
	}

	protected abstract TWriter GetWriter(OutputFormatterWriteContext context);

	protected override bool CanWriteType(Type? type) => typeof(JsonStream).IsAssignableFrom(type);

	public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
	{
		var cancellationToken = context.HttpContext.RequestAborted;

		var jsonStream = (JsonStream)context.Object!;

		TWriter? writer = null;
		try
		{
			context.HttpContext.Response.ContentType = ContentType;
			using StreamReader reader = new StreamReader(jsonStream);
			writer = GetWriter(context);
			var converter = context.HttpContext.RequestServices.GetService<TConverter>();
			await converter!.ConvertAsync(reader, writer, cancellationToken);
		}
		finally
		{
			await TryDisposeWriter(writer);
			jsonStream.Dispose();
		}
	}

	/// <summary>
	/// Accepts response body writer as parameter and tries to dispose it if possible.
	/// </summary>
	/// <param name="writer">Writer used to write to response body.</param>
	private async Task TryDisposeWriter(TWriter? writer)
	{
		if (writer == null)
			return;

		if (writer is IAsyncDisposable asyncDisposable)
		{
			await asyncDisposable.DisposeAsync();
		}
		else if (writer is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}

}