using Domain.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Activities.Queries;
using Application.Activities.Commands;
using Application.Core;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{

    public class ActivitiesController : BaseApiController
    {
       

        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            return HandleResult(await Mediator.Send(new ListActivitesQuery.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
          
            return HandleResult(await Mediator.Send(new ActivityDetailsQuery.Query { Id = id }));

		}

        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromBody]Activity activity)
        {
           return HandleResult(await Mediator.Send(new CreateActivityCommand.Command { Activity=activity }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, [FromBody]Activity activity)
        {
            activity.Id = id;
            return HandleResult( await Mediator.Send(new EditActivityCommand.Command { Activity=activity }));
        }

		[Authorize(Policy = "IsActivityHost")]
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteActivityCommand.Command { Id=id }));
           
        }

        [HttpPost("{id}/attend")]
		public async Task<IActionResult> Attend(Guid id)
		{
			return HandleResult(await Mediator.Send(new UpdateAttendanceCommand.Command { Id = id }));

		}

	}
}
