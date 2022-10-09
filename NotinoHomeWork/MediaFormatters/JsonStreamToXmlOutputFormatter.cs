using Microsoft.AspNetCore.Mvc.Formatters;
using NotinoHomeWork.Application.Services;

namespace NotinoHomeWork.API.MediaFormatters;

/// <summary>
/// Output formatter from <see cref="JsonStream"/> to XML.
/// </summary>
public class JsonStreamToXmlOutputFormatter : FromJsonOutputFormatterBase<StreamWriter, JsonToXmlConverter>
{
	protected override string ContentType => "application/xml";
	protected override StreamWriter GetWriter(OutputFormatterWriteContext context) => new(context.HttpContext.Response.Body);
}