using Microsoft.AspNetCore.Mvc.Formatters;

namespace NotinoHomeWork.API.MediaFormatters;

/// <summary>
/// Output formatter from <see cref="JsonStream"/> to JSON.
/// </summary>
public class JsonStreamToJsonOutputFormatter : OutputFormatter
{
	public JsonStreamToJsonOutputFormatter()
	{
		SupportedMediaTypes.Add("application/json");
	}

	protected override bool CanWriteType(Type? type) => typeof(JsonStream).IsAssignableFrom(type);

	public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
	{
		Stream stream = (JsonStream)context.Object!;

		try
		{
			// Just copy the JsonStream to Body of a response.
			context.HttpContext.Response.ContentType = "application/json";
			await stream.CopyToAsync(context.HttpContext.Response.Body);
			await context.HttpContext.Response.BodyWriter.FlushAsync();
		}
		finally
		{
			await stream.DisposeAsync();
		}
	}
}