using Application.Photos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IPhotoAccessor
	{
		Task<PhotoUploadResult> AddPhoto(IFormFile photo);
		Task<string> DeletePhoto(string publicId);
	}
}
