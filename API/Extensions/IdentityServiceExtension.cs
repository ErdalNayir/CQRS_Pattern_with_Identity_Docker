using API.Services;
using Domain.Concrete;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Persistance.Concrete;
using System.Text;

namespace API.Extensions
{
	public static class IdentityServiceExtension
	{
		public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
		{
			services.AddIdentityCore<AppUser>(opt =>
			{
				opt.Password.RequireNonAlphanumeric = false;
				opt.User.RequireUniqueEmail = true;


			}).AddEntityFrameworkStores<DataContext>();

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opt =>
				{
					opt.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = key,
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateLifetime = true, // ekstradan ekledim
					};
				});


			services.AddAuthorization(opt =>
			{
				opt.AddPolicy("IsActivityHost", policy =>
				{
					policy.AddRequirements(new IsHostRequirement());
				});
			});
			services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
			services.AddScoped<TokenService>();

			return services;
		}
	}
}
