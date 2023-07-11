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
	public class DeletePhotoCommand
	{
		public class Command : IRequest<Result<Unit>>
		{
			public string Id { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly DataContext _dataContext;
			private readonly IUserAccessor _userAccessor;
			private readonly IPhotoAccessor _photoAccessor;

			public Handler(DataContext dataContext, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
            {
				_dataContext = dataContext;
				_userAccessor = userAccessor;
				_photoAccessor = photoAccessor;
			}
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _dataContext.Users.Include(x=>x.Photos).FirstOrDefaultAsync(p=>p.UserName == _userAccessor.GetUsername());

				if (user == null) return null;

				var photo = user.Photos.FirstOrDefault(x =>x.Id ==  request.Id);

				if (photo == null) return null;

				if (photo.IsMain) return Result<Unit>.Failure("You cannot delete your main photo");

				var result = await _photoAccessor.DeletePhoto(photo.Id);

				if (result == null) Result<Unit>.Failure("Error deleting photo from cloudinary");

				user.Photos.Remove(photo);

				var success = await _dataContext.SaveChangesAsync() > 0;

				if (success) return Result<Unit>.Success(Unit.Value);

				return Result<Unit>.Failure("Error deleting photo from API");

			}
		}
	}
}
