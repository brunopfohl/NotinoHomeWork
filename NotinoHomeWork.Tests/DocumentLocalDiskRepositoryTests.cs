using Moq;
using NotinoHomeWork.Application.Errors;
using NotinoHomeWork.Application.Models;
using NotinoHomeWork.Application.Services;
using NotinoHomeWork.Services;
using Xunit;

namespace NotinoHomeWork.Tests;

public class DocumentLocalDiskRepositoryTests
{

	private readonly DocumentLocalDiskRepository _sut;
	private Mock<IFileService> _fileServiceMock = new();

	public DocumentLocalDiskRepositoryTests()
	{
		_sut = new DocumentLocalDiskRepository(_fileServiceMock.Object);
	}

	[Fact]
	public async Task GetRawAsync_OnFileNotFoundException_ReturnsFileNotFoundError()
	{
		// Arrange
		var documentId = "identifier";
		var path = DocumentLocalDiskRepository.GetPath(documentId);

		_fileServiceMock
			.Setup(x => x.OpenRead(path))
			.Throws<FileNotFoundException>();

		// Act
		var res = await _sut.GetRawAsync(documentId, CancellationToken.None);

		string errorMessage =
			$"{nameof(DocumentLocalDiskRepository.GetRawAsync)} should have returned {nameof(DocumentNotFoundError)}.";

		// Assert
		res.Switch(
			s => Assert.Fail(errorMessage),
			notFoundError => {},
			UnknownError => Assert.Fail(errorMessage)
		);
	}

	[Fact]
	public async Task GetRawAsync_OnAnyOtherException_ReturnsUnknownError()
	{
		// Arrange
		var documentId = "identifier";
		var path = DocumentLocalDiskRepository.GetPath(documentId);

		_fileServiceMock
			.Setup(x => x.OpenRead(path))
			.Throws<Exception>();

		// Act
		var res = await _sut.GetRawAsync(documentId, CancellationToken.None);

		string errorMessage =
			$"{nameof(DocumentLocalDiskRepository.GetRawAsync)} should have returned {nameof(UnknownError)}.";

		// Assert
		res.Switch(
			s => Assert.Fail(errorMessage),
			notFoundError => Assert.Fail(errorMessage),
			unknownError => {}
		);
	}

	[Fact]
	public async Task InsertAsync_WhenFileAlreadyExists_ReturnsDocumentAlreadyExistsError()
	{
		// Arrange
		var document = new Document()
		{
			Id = "identifier",
			Tags = new[] { "Tag1" },
			Data = new
			{
				a = "1"
			}
		};

		var path = DocumentLocalDiskRepository.GetPath(document.Id);

		_fileServiceMock
			.Setup(x => x.Exists(path))
			.Returns(true);

		// Act
		var res = await _sut.InsertAsync(document, CancellationToken.None);

		string errorMessage =
			$"{nameof(DocumentLocalDiskRepository.InsertAsync)} should have returned {nameof(DocumentAlreadyExistsError)}.";

		// Assert
		res.Switch(
			s => Assert.Fail(errorMessage),
			alreadyExistsError => {}
		);
	}

	[Fact]
	public async Task UpdateAsync_WhenDocumentDoesNotExist_ReturnsDocumentNotFoundError()
	{
		// Arrange
		var document = new Document()
		{
			Id = "identifier",
			Tags = new[] { "Tag1" },
			Data = new
			{
				a = "1"
			}
		};

		var path = DocumentLocalDiskRepository.GetPath(document.Id);

		_fileServiceMock
			.Setup(x => x.Exists(path))
			.Returns(false);

		// Act
		var res = await _sut.UpdateAsync(document, CancellationToken.None);

		string errorMessage =
			$"{nameof(DocumentLocalDiskRepository.UpdateAsync)} should have returned {nameof(DocumentNotFoundError)}.";

		// Assert
		res.Switch(
			s => Assert.Fail(errorMessage),
			documentNotFoundError => {}
		);
	}
}