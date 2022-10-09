using NotinoHomeWork.Application.Services;
using System.Text;
using NotinoHomeWork.Tests.Extensions;
using Xunit;

namespace NotinoHomeWork.Tests;

public class JsonToXmlConverterTests
{
	private readonly JsonToXmlConverter _sut = new();

	[Fact]
	public async Task JsonToXml_WhenInputJsonStreamIsValid_WriteValidXml()
	{
		// Arrange
		var json = @"{ ""id"" : ""identifier"", ""tags"": [""tag1, tag2""], data: { ""a"": ""1"", ""b"": ""2"", ""c"": ""3"" }}";
		using var jsonStream = json.ToStream();
		using var jsonStreamReader = new StreamReader(jsonStream);

		using var xmlStream = new MemoryStream();
		using var xmlStreamWriter = new StreamWriter(xmlStream);
		using var xmlStreamReader = new StreamReader(xmlStream);

		var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <id>identifier</id>
  <tags>tag1, tag2</tags>
  <data>
    <a>1</a>
    <b>2</b>
    <c>3</c>
  </data>
</root>";

		// Act
		await _sut.ConvertAsync(jsonStreamReader, xmlStreamWriter, CancellationToken.None);
		xmlStream.Seek(0, SeekOrigin.Begin);
		var actualXml = await xmlStreamReader.ReadToEndAsync();

		// Assert
		Assert.Equal(actualXml, expectedXml);
	}
}