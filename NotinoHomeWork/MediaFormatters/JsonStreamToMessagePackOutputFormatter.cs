using System.Buffers;
using System.IO.Pipelines;
using Microsoft.AspNetCore.Mvc.Formatters;
using NotinoHomeWork.Application.Services;

namespace NotinoHomeWork.API.MediaFormatters;

/// <summary>
/// Output formatter from <see cref="JsonStream"/> to MessagePack.
/// See https://msgpack.org/ for reference.
/// </summary>
public class JsonStreamToMessagePackOutputFormatter : FromJsonOutputFormatterBase<IBufferWriter<byte>, JsonToMessagePackConverter>
{
	protected override string ContentType => "application/msgpack";
	protected override PipeWriter GetWriter(OutputFormatterWriteContext context) => context.HttpContext.Response.BodyWriter;
}