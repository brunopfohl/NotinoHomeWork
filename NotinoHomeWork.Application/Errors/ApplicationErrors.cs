namespace NotinoHomeWork.Application.Errors;

public static class ApplicationErrors
{
	/// <summary>
	/// Document was not found in the data store.
	/// </summary>
	public static DocumentNotFoundError DocumentNotFound = new();

	/// <summary>
	/// Operation crashed due to an unspecified error.
	/// </summary>
	public static UnknownError UnknownError = new();

	/// <summary>
	/// New document cannot be uploaded, it already exists.
	/// </summary>
	public static DocumentAlreadyExistsError DocumentAlreadyExists = new();
}