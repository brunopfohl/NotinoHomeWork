using NotinoHomeWork.Application.Errors;
using NotinoHomeWork.Application.Models;
using OneOf;

namespace NotinoHomeWork.Application.Services;

/// <summary>
/// Repository for managing documents.
/// </summary>
public interface IDocumentRepository
{
	/// <summary>
	/// Gets JSON document as a stream if possible, otherwise <see cref="DocumentNotFoundError"/> or <see cref="UnknownError"/> is returned.
	/// </summary>
	/// <param name="id">Identifier of a document</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>JSON document as a stream if possible, <see cref="DocumentNotFoundError"/> or <see cref="UnknownError"/> otherwise.</returns>
	Task<OneOf<JsonStream, DocumentNotFoundError, UnknownError>> GetRawAsync(string id, CancellationToken cancellationToken);

	/// <summary>
	/// Inserts new document in to the data store.
	/// </summary>
	/// <param name="document">Document to be stored.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Number of uploaded documents, <see cref="DocumentAlreadyExistsError"/> if otherwise.</returns>
	Task<OneOf<int, DocumentAlreadyExistsError>> InsertAsync(Document document, CancellationToken cancellationToken);

	/// <summary>
	/// Updates already existing document in the data store.
	/// </summary>
	/// <param name="document">New form of the document.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Number of changed documents, <see cref="DocumentNotFoundError"/> otherwise.</returns>
	Task<OneOf<int, DocumentNotFoundError>> UpdateAsync(Document document, CancellationToken cancellationToken);
}