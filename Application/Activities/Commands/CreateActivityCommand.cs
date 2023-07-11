using Application.Activities.Validations;
using Application.Core;
using Application.Interfaces;
using Domain.Concrete;
using FluentValidation;
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
    public class CreateActivityCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator :  AbstractValidator<Command> { 

            public CommandValidator() {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());

            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>> 
        {
            private readonly DataContext _dataContext;
			private readonly IUserAccessor _userAccessor;

			public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
				_userAccessor = userAccessor;
			}

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x=>x.UserName == _userAccessor.GetUsername());
              

                var attendee = new ActivityAttendee
                {
                    AppUser = user,
                    Activity = request.Activity,
                    IsHost = true
                };

                request.Activity.Attendees.Add(attendee);

				_dataContext.Activities.Add(request.Activity);

				var result =  await _dataContext.SaveChangesAsync() > 0 ;

                if (!result) return Result<Unit>.Failure("Failed to create activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
