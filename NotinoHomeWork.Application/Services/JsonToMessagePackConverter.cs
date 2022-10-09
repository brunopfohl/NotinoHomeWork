using System.Buffers;
using MessagePack;
using MessagePack.Resolvers;

namespace NotinoHomeWork.Application.Services;

/// <summary>
/// Converter from JSON to MessagePack.
/// See https://msgpack.org/ for reference.
/// </summary>
public class JsonToMessagePackConverter : IConverterService<IBufferWriter<byte>>
{
	public async Task ConvertAsync(StreamReader reader, IBufferWriter<byte> writer, CancellationToken cancellationToken)
	{
		void Write()
		{
			var msgPackWriter = new MessagePackWriter(writer);

			// Unfortunatelly, following method does not have option to pass cancellationToken.
			MessagePackSerializer.ConvertFromJson(reader, ref msgPackWriter, ContractlessStandardResolver.Options);

			// There is no need to continue, if cancellation was requested.
			if (cancellationToken.IsCancellationRequested)
				return;

			msgPackWriter.Flush();
		}

		Write();
	}
}