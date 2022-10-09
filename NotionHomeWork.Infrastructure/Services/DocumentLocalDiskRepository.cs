using System.Reflection;
using System.Text.Json;
using NotinoHomeWork.Application.Errors;
using NotinoHomeWork.Application.Models;
using NotinoHomeWork.Application.Services;
using OneOf;

namespace NotinoHomeWork.Services;

public class DocumentLocalDiskRepository : IDocumentRepository
{
	private readonly IFileService _fileService;

	public DocumentLocalDiskRepository(IFileService fileService)
	{
		_fileService = fileService;
		Init();
	}

	private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
	{
		WriteIndented = true
	};

	public async Task<OneOf<JsonStream, DocumentNotFoundError, UnknownError>> GetRawAsync(string id, CancellationToken cancellationToken)
	{
		// In this scenario there's no need to react to the cancellationToken.

		try
		{
			return new JsonStream(_fileService.OpenRead(GetPath(id)));
		}
		catch (FileNotFoundException)
		{
			return ApplicationErrors.DocumentNotFound;
		}
		catch (Exception)
		{
			return ApplicationErrors.UnknownError;
		}
	}

	public async Task<OneOf<int, DocumentAlreadyExistsError>> InsertAsync(Document document, CancellationToken cancellationToken)
	{
		if (_fileService.Exists(GetPath(document.Id)))
			return ApplicationErrors.DocumentAlreadyExists;

		await WriteDocument(document, cancellationToken);

		return 1;
	}

	public async Task<OneOf<int, DocumentNotFoundError>> UpdateAsync(Document document, CancellationToken cancellationToken)
	{
		if (!_fileService.Exists(GetPath(document.Id)))
			return ApplicationErrors.DocumentNotFound;

		await WriteDocument(document, cancellationToken);

		return 1;
	}

	private async Task WriteDocument(Document document, CancellationToken cancellationToken)
	{
		await using var stream = _fileService.OpenWrite(GetPath(document.Id));
		await JsonSerializer.SerializeAsync(stream, document, _jsonSerializerOptions, cancellationToken);
	}

	public static string GetPath(string documentId)
	{
		return $"{GetStorageFolder()}{documentId}.json";
	}

	private static void Init() => Directory.CreateDirectory(GetStorageFolder());

	private static string GetStorageFolder()
	{
		var currentFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
		return Path.Combine(currentFolder, "Documents");
	}
}