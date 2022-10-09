using System.ComponentModel.DataAnnotations;

namespace NotinoHomeWork.Application.Models;

/// <summary>
/// Document model.
/// </summary>
public class Document : IValidatableObject
{
	/// <summary>
	/// Get's or sets Id of a document.
	/// </summary>
	public string Id { get; set; }

	/// <summary>
	/// Get's or sets tags of a document.
	/// </summary>
	public string[] Tags { get; set; }

	/// <summary>
	/// Get's or sets the actual data of a document.
	/// </summary>
	public object Data { get; set; }

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (string.IsNullOrWhiteSpace(Id))
		{
			yield return new ValidationResult($"{Id} is required.");
		}
	}
}