using Application.Core;
using Application.Interfaces;
using Domain.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Commands
{
	public class UpdateAttendanceCommand
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid Id { get; set; }

		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly DataContext _context;
			private readonly IUserAccessor _userAccessor;

			public Handler(DataContext context, IUserAccessor userAccessor)
            {
				_context = context;
				_userAccessor = userAccessor;
			}
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var activity = await _context.Activities.Include(x=>x.Attendees)
					.ThenInclude(y=>y.AppUser)
					.SingleOrDefaultAsync(t=>t.Id == request.Id);

				if (activity == null) return null;

				var user = await _context.Users.FirstOrDefaultAsync(x => 
					x.UserName == _userAccessor.GetUsername());

				if (user == null) return null;

				var hostName = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

				var attendance = activity.Attendees.FirstOrDefault(x=>x.AppUser.UserName == user.UserName);

				if(attendance!=null && hostName == user.UserName)
				{
					activity.IsCancelled = !activity.IsCancelled;
				}

				if(attendance != null && hostName != user.UserName)
				{
					activity.Attendees.Remove(attendance);
				}

				if(attendance == null) 
				{
					attendance = new ActivityAttendee
					{
						AppUser = user,
						Activity = activity,
						IsHost = false
					};

					activity.Attendees.Add(attendance);
				}

				var result = await _context.SaveChangesAsync() > 0;

				return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem Updating Attendance");

			}
		}
	}
}
