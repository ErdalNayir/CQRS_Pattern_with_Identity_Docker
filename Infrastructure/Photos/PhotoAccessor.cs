using Application.Interfaces;
using Application.Photos;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Photos
{
	public class PhotoAccessor : IPhotoAccessor
	{
		private readonly Cloudinary _cloudinary;

        public PhotoAccessor(IOptions<CloudinarySettings> config)
        {
			var account = new Account
				(
					config.Value.CloudName, 
					config.Value.ApiKey, 
					config.Value.ApiSecret
				);
        }
        public async Task<PhotoUploadResult> AddPhoto(IFormFile photo)
		{
			if(photo.Length > 0)
			{
				await using var stream = photo.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(photo.FileName, stream),
					Transformation = new Transformation().Height(512).Width(512).Crop("fill")
				};

				var uploadResult = await _cloudinary.UploadAsync(uploadParams);

				if(uploadResult.Error != null)
				{
					throw new Exception(uploadResult.Error.Message);
				}

				return new PhotoUploadResult
				{
					PublicId = uploadResult.PublicId,
					Url = uploadResult.SecureUrl.ToString()
				};
			}
			return null;
		}

		public async Task<string> DeletePhoto(string publicId)
		{
			var deleteParams = new DeletionParams(publicId);

			var result = await _cloudinary.DestroyAsync(deleteParams);

			return result.Result == "ok" ? result.Result : null;
		}
	}
}
