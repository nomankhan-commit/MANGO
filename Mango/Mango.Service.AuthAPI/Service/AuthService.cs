using Mango.Service.AuthAPI.Model;
using Mango.Service.AuthAPI.Model.Dto;
using Mango.Service.AuthAPI.Service.IService;
using Mango.Services.AuthApi.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Mango.Service.AuthAPI.Service
{
	public class AuthService : IAuthService
	{

		private readonly AppDbContext _db;
		private readonly UserManager<ApplicationUsers> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly  IJWTokenGenerator _jwtGenerator;

		public AuthService(AppDbContext db,
			UserManager<ApplicationUsers> userManager, 
			RoleManager<IdentityRole> roleManager,
			IJWTokenGenerator jWTokenGenerator) { 
		
			_db = db;
			_userManager = userManager;
			_roleManager = roleManager;
			_jwtGenerator = jWTokenGenerator;

		}

		public async Task<bool> AssignRole(string email, string roleName)
		{
			var user = _db.ApplicationUsers.FirstOrDefault(u=>u.Email.ToLower() == email.ToLower());
			if (user != null)
			{
				if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
				{
				  	_roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
				}
				await _userManager.AddToRoleAsync(user,roleName);
				return true;	
			}
			return false;
		}

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
		{

			var user = _db.ApplicationUsers.First(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
			bool valid = await _userManager.CheckPasswordAsync(user,loginRequestDto.Password);
			if (user == null || !valid)
			{
				return new LoginResponseDto(){User = null, Token="" };
			}

			var roles = await _userManager.GetRolesAsync(user);
			var token = _jwtGenerator.GenerateToken(user,roles);
			UserDto userDto = new UserDto() {
			
				Email = user.Email,
				Id= user.Id,
				PhnoneNumber = user.PhoneNumber,
				Name = user.Name
			
			};

			LoginResponseDto loginResponseDto = new LoginResponseDto() {
				User = userDto,
				Token = token

			};
			return loginResponseDto;
			
		}

		public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
		{
			ApplicationUsers user = new() {

				UserName = registrationRequestDto.Email,
				Email = registrationRequestDto.Email,
				NormalizedEmail = registrationRequestDto.Email.ToUpper(),
				Name = registrationRequestDto.Name,
				PhoneNumber = registrationRequestDto.PhoneNumber
			};
			try
			{
				var result = await _userManager.CreateAsync(user,registrationRequestDto.Password);
				if (result.Succeeded)
				{
					var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);
					UserDto userDto = new() {

						Id = userToReturn.Id,
						Email = userToReturn.Email,
						Name = userToReturn.Name,
						PhnoneNumber = userToReturn.PhoneNumber

					};
					//return userDto;
					return "";
				}
				else
				{
					return result.Errors.FirstOrDefault().Description;
				}
			}
			catch (Exception ex)
			{

				
			}


			return "Error!";
		}


        //public async Task SingInUser(LoginResponseDto model)
        //{

        //    var handler = new JwtSecurityTokenHandler();
        //    var jwt = handler.ReadJwtToken(model.Token);

        //    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        //    identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
        //        jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

        //    identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
        //        jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

        //    identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
        //        jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

        //    identity.AddClaim(new Claim(ClaimTypes.Name,
        //        jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));




        //    var principle = new ClaimsPrincipal(identity);
        //    //await HttpContext.s(CookieAuthenticationDefaults.AuthenticationScheme, principle);
        //    await Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.SignInAsync(null, CookieAuthenticationDefaults.AuthenticationScheme, principle);
        //}

    }
}
