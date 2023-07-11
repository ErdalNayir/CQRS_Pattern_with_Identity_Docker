using Application.Profiles.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	
	public class ProfilesController : BaseApiController
	{
		[HttpGet("{username}")]
		public async Task<IActionResult> GetProfile(string username)
		{
			return HandleResult(await Mediator.Send(new ProfileDetailQuery.Query { Username = username }));
		}
	}
}
