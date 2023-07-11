using Application.Activities.Validations;
using Application.Core;
using AutoMapper;
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
    public class EditActivityCommand
    {
        public class Command : IRequest<Result<Unit>>

        {
            public Activity Activity { get; set; }
        }

		public class CommandValidator : AbstractValidator<Command>
		{

			public CommandValidator()
			{
				RuleFor(x => x.Activity).SetValidator(new ActivityValidator());

			}
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _dataContext = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities.FindAsync(request.Activity.Id);

                if (activity == null) return null;

                _mapper.Map(request.Activity, activity);

                var result =  await _dataContext.SaveChangesAsync() > 0 ;

                if (!result) return Result<Unit>.Failure("Failed to edit activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
