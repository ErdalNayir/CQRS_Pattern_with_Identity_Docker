using API.DTOs;
using API.Services;
using Application.Core;
using Domain.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
	
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly TokenService _tokenService;

		public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
			_userManager = userManager;
			_tokenService = tokenService;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await _userManager.Users.Include(x=>x.Photos).FirstOrDefaultAsync(y=>y.Email==loginDto.Email);

			if (user == null) return Unauthorized();

			var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

			if (result)
			{
				return UserDtoCreator(user);
			}

			return Unauthorized();
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username)) return BadRequest("Username is already taken");

			var user = new AppUser
			{
				DisplayName = registerDto.DisplayName,
				UserName = registerDto.Username,
				Email = registerDto.Email,
			};

			var result = await _userManager.CreateAsync(user,registerDto.Password);

			if (result.Succeeded)
			{
				return UserDtoCreator(user);
			}

			return BadRequest("There is a problem registering user");
		}


		[Authorize]
		[HttpGet]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var user = await _userManager.Users.Include(x => x.Photos).FirstOrDefaultAsync(y => y.Email == ClaimTypes.Email);
			return UserDtoCreator(user);
		}


		//ITS NOT A ENDPOINT
		private UserDto UserDtoCreator(AppUser user)
		{
			return new UserDto
			{
				DisplayName = user.DisplayName,
				Username = user.DisplayName,
				Image = user?.Photos?.FirstOrDefault(x=>x.IsMain)?.Url,
				Token = _tokenService.CreateToken(user)
			};
		}
	}
}
