using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Photos
{
	public class SetMainCommand
	{
		public class Command : IRequest<Result<Unit>>
		{
            public string  Id { get; set; }
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
				var user = await _dataContext.Users.Include(x=>x.Photos).FirstOrDefaultAsync(p=>p.UserName == _userAccessor.GetUsername());

				if (user == null) return null;

				var photo = user.Photos.FirstOrDefault(x=>x.Id == request.Id);

				if (photo == null) return null;

				var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

				if(currentMain!= null) currentMain.IsMain = false;

				photo.IsMain = true;

				var success = await _dataContext.SaveChangesAsync(cancellationToken) >0;

				if (success) return Result<Unit>.Success(Unit.Value);

				return Result<Unit>.Failure("Problem setting main photo");
			}
		}
	}
}
