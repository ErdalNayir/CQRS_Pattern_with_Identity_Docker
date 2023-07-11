using Application.Photos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class PhotosController : BaseApiController
	{
		[HttpPost]
		public async Task<IActionResult> AddPhoto([FromForm] AddPhotoCommand.Command command)
		{
			return HandleResult(await Mediator.Send(command));
		}

		[HttpPost("{id}")]
		public async Task<IActionResult> DeletePhoto(string id)
		{
			return HandleResult(await Mediator.Send(new DeletePhotoCommand.Command { Id = id}));
		}

		[HttpPost("{id}/setMain")]
		public async Task<IActionResult> SetMainPhoto(string id)
		{
			return HandleResult(await Mediator.Send(new SetMainCommand.Command { Id = id }));
		}

	}
}
