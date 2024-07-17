using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using Mango.web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.JSInterop.Implementation;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;






namespace Mango.web.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService,ITokenProvider tokenProvider)
        {
		     _authService = authService;
			_tokenProvider = tokenProvider;
        }


        [HttpGet]
		public IActionResult Login()
		{
            
			return View();
		}

        [HttpPost]
		public async Task<IActionResult> Login(LoginRequestDto obj) {

            ResponseDto result = await _authService.LoginAsync(obj);

            if (result != null && result.Sussess)
            {
				LoginResponseDto loginResponseDto = JsonConvert.
					DeserializeObject<LoginResponseDto>(Convert.ToString(result.Result));
                await SingInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                TempData["success"] = "Success!";
                return RedirectToAction("Index", "Home");
            }
			else
			{
                TempData["error"] = result.Message;
                ModelState.AddModelError("CustomError",result.Message);
                return View(obj);
            }
      
        }


		[HttpGet]
		public IActionResult Register()
		{
            InItRoleDDl();
            return View();
		}
		
		[HttpPost]
		public async Task<IActionResult> Register(RegistrationRequestDto obj)
		{

	        ResponseDto result =  await _authService.RegisterAsync(obj);
			
			if (result != null && result.Sussess) {
                ResponseDto assignRole;
                if (string.IsNullOrEmpty(obj.Role))
				{
					obj.Role = SD.RoleCustomer;
				}
				assignRole = await _authService.AssingRoleAsync(obj);
				if (assignRole != null && assignRole.Sussess)
				{
					TempData["success"] = "Registration Successful";
					return RedirectToAction(nameof(Login));
				}
            }
            else
            {
                TempData["error"] = result.Message;
            }
			InItRoleDDl();
            return View(obj);
		}

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");   
        }


		void InItRoleDDl() {
            var roleList = new List<SelectListItem>() {
                new SelectListItem {Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem {Text = SD.RoleCustomer, Value = SD.RoleCustomer },
            };
            ViewBag.RoleList = roleList;
        }

        public async Task SingInUser(LoginResponseDto model)
        {

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));




            var principle = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
            //await Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.SignInAsync(null, CookieAuthenticationDefaults.AuthenticationScheme, principle);
        }
    }
}
