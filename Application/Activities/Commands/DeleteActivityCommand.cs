using Application.Core;
using MediatR;
using Persistance.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Commands
{
    public class DeleteActivityCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);

                if (activity == null) return null;

                _context.Remove(activity);

                var result = await _context.SaveChangesAsync() > 0 ;

                if (!result) return Result<Unit>.Failure("Failed to delete activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
