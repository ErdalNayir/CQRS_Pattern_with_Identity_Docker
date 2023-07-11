using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
	public class UserAccessor : IUserAccessor
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
			_httpContextAccessor = httpContextAccessor;
		}
        public string GetUsername()
		{
			return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
		}
	}
}
