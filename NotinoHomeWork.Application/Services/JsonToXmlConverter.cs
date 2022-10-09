using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NotinoHomeWork.Application.Services;

/// <summary>
/// Converter from JSON to XML.
/// </summary>
public class JsonToXmlConverter : IConverterService<StreamWriter>
{
	public async Task ConvertAsync(StreamReader reader, StreamWriter writer, CancellationToken cancellationToken)
	{
		var xNode = LoadXNode(reader, "root");
		await xNode.SaveAsync(writer, SaveOptions.None, cancellationToken);

		// There is no need to continue, if cancellation was requested.
		if (cancellationToken.IsCancellationRequested)
			return;

		await writer.FlushAsync();
	}

	private static XDocument? LoadXNode(TextReader textReader, string deserializeRootElementName)
	{
		var settings = new JsonSerializerSettings
		{
			Converters = { new XmlNodeConverter { DeserializeRootElementName = deserializeRootElementName } }
		};

		using var jsonReader = new JsonTextReader(textReader) { CloseInput = false };
		return JsonSerializer.CreateDefault(settings).Deserialize<XDocument>(jsonReader);
	}
}