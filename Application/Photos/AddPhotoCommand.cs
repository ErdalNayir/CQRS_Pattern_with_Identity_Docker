using Application.Core;
using Application.Interfaces;
using Domain.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistance.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Photos
{
	public class AddPhotoCommand
	{
		public class Command: IRequest<Result<Photo>> 
		{
            public IFormFile File { get; set; }

        }

		public class Handler : IRequestHandler<Command, Result<Photo>>
		{
			private readonly DataContext _context;
			private readonly IPhotoAccessor _photoAccessor;
			private readonly IUserAccessor _userAccessor;

			public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
				_context = context;
				_photoAccessor = photoAccessor;
				_userAccessor = userAccessor;
			}

            public async  Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x=>x.UserName == _userAccessor.GetUsername());

				if (user == null) return null;

				var PhotoUploadResult = await _photoAccessor.AddPhoto(request.File);

				var photo = new Photo
				{
					Url = PhotoUploadResult.Url,
					Id = PhotoUploadResult.PublicId
				};

				if(!user.Photos.Any(x=>x.IsMain)) photo.IsMain = true;

				user.Photos.Add(photo);

				var result = await _context.SaveChangesAsync() > 0 ;

				if(result) return Result<Photo>.Success(photo);

				return Result<Photo>.Failure("Error uploading photo");


			}
		}
	}
}
