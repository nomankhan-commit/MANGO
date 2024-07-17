using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using Mango.web.Utility;

namespace Mango.web.Service
{
	public class AuthService : IAuthService
	{
		private readonly IBaseService _baseService;
		public AuthService(IBaseService baseService) { 
		       _baseService = baseService;
		}



		public async Task<ResponseDto> AssingRoleAsync(RegistrationRequestDto registrationRequestDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = registrationRequestDto,
				url = SD.AuthApiBase + "/api/AuthApi/AssingRole"
            }, withBearer: false); 
		}

		public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = loginRequestDto,
				url = SD.AuthApiBase + "/api/AuthApi/Login"
            },withBearer:false);
		}

		public async Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = registrationRequestDto,
				url = SD.AuthApiBase + "/api/AuthApi/Register"
            },withBearer:false);
		}
	}
}
