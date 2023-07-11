using Domain.Concrete;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
	public class TokenService
	{
		private readonly IConfiguration _config;

		public TokenService(IConfiguration config) 
		{
			_config = config;
		}

		public string CreateToken(AppUser appUser)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, appUser.UserName),
				new Claim(ClaimTypes.NameIdentifier, appUser.Id),
				new Claim(ClaimTypes.Email, appUser.Email),
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = creds

			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
