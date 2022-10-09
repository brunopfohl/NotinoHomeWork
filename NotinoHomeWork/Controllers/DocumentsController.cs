using Microsoft.AspNetCore.Mvc;
using NotinoHomeWork.API.ActionResults;
using NotinoHomeWork.Application.Models;
using NotinoHomeWork.Application.Services;

namespace NotinoHomeWork.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentsController : ControllerBase
{
	private readonly IDocumentRepository _documentRepository;

	public DocumentsController(IDocumentRepository documentRepository)
	{
		_documentRepository = documentRepository;
	}

	/// <summary>
	/// Accepts <see cref="Document"/> and saves it to the data store.
	/// </summary>
	/// <param name="document">Document to be stored/uploaded.</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>No content.</returns>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Post(Document document, CancellationToken cancellationToken)
	{
		if (ValidationFailed(out var failedActionResult))
			return failedActionResult!;

		var result = await _documentRepository.InsertAsync(document, cancellationToken);

		return result.Match<IActionResult>(
			changedCount => Ok(),
			documentAlreadyExists => StatusCode(500)
		);
	}

	/// <summary>
	/// Gets content of saved document. Can produce multiple types of content (see OutputTypeFormatters in Program.cs).
	/// </summary>
	/// <param name="id">Id of a document</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Content of an already stored document.</returns>
	[HttpGet]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
	{
		var rawJsonDocument = await _documentRepository.GetRawAsync(id, cancellationToken);

		return rawJsonDocument.Match<IActionResult>(
			stream => new JsonStreamActionResult(stream),
			documentNotFoundError => NotFound(),
			unknownError => StatusCode(500)
		);
	}

	/// <summary>
	/// Updates an already existing document.
	/// </summary>
	/// <param name="document">Document to be updated</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>No content.</returns>
	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Put(Document document, CancellationToken cancellationToken)
	{
		if (ValidationFailed(out var failedActionResult))
			return failedActionResult!;

		var result = await _documentRepository.UpdateAsync(document, cancellationToken);

		return result.Match<IActionResult>(
			changedCount => Ok(),
			documentAlreadyExists => StatusCode(500)
		);
	}

	private bool ValidationFailed(out IActionResult? failedActionResult)
	{
		if (!ModelState.IsValid)
		{
			failedActionResult = BadRequest(ModelState.Values.SelectMany(v => v.Errors));
			return true;
		}

		failedActionResult = null;
		return false;
	}
}