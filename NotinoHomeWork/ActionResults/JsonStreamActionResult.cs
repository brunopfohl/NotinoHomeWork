using Microsoft.AspNetCore.Mvc;

namespace NotinoHomeWork.API.ActionResults;

public class JsonStreamActionResult : IActionResult
{

	public JsonStreamActionResult(JsonStream jsonStream)
	{
		JsonStream = jsonStream;
	}

	public JsonStream JsonStream { get; }

	public async Task ExecuteResultAsync(ActionContext context)
	{
		var objectResult = new ObjectResult(JsonStream)
		{
			StatusCode = StatusCodes.Status200OK
		};

		await objectResult.ExecuteResultAsync(context);
	}
}