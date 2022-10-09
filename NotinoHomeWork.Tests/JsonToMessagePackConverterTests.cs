using System.IO.Pipelines;
using NotinoHomeWork.Application.Services;
using NotinoHomeWork.Tests.Extensions;
using Xunit;

namespace NotinoHomeWork.Tests;

public class JsonToMessagePackConverterTests
{
	private readonly JsonToMessagePackConverter _sut = new();

	[Fact]
	public async Task JsonToMessagePack_WhenInputJsonStreamIsValid_WriteValidMessagePack()
	{

		// Arrange
		var json = @"{ ""id"" : ""identifier"", ""tags"": [""tag1"", ""tag2""], ""data"": { ""a"": ""1"", ""b"": ""2"", ""c"": ""3"" }}";
		using var jsonStream = json.ToStream();
		using var jsonStreamReader = new StreamReader(jsonStream);

		using var msgPackStream = new MemoryStream();
		var msgPackStreamWriter = PipeWriter.Create(msgPackStream);
		using var msgPackBinaryReader = new BinaryReader(msgPackStream);

		// Expected message pack bytes were extracted from https://msgpack.org/ ("Try! section")
		byte[] expectedMessagePack =
		{
			131, 162, 105, 100, 170, 105, 100, 101, 110, 116, 105, 102, 105, 101, 114, 164, 116, 97, 103, 115, 146, 164,
			116, 97, 103, 49, 164, 116, 97, 103, 50, 164, 100, 97, 116, 97, 131, 161, 97, 161, 49, 161, 98, 161, 50,
			161, 99, 161, 51
		};

		// Act
		await _sut.ConvertAsync(jsonStreamReader, msgPackStreamWriter, CancellationToken.None);
		await msgPackStreamWriter.FlushAsync(CancellationToken.None);
		msgPackStream.Seek(0, SeekOrigin.Begin);

		// Assert
		Assert.Equal(expectedMessagePack.Length, msgPackBinaryReader.BaseStream.Length);

		while (msgPackBinaryReader.BaseStream.Position < msgPackBinaryReader.BaseStream.Length)
		{
			byte expectedByte = expectedMessagePack[msgPackBinaryReader.BaseStream.Position];
			byte actualByte = msgPackBinaryReader.ReadByte();
			Assert.Equal(actualByte, expectedByte);
		}
	}
}