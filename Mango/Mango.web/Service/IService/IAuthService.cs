using Mango.web.Models.Dto;
using Microsoft.AspNetCore.Identity.Data;

namespace Mango.web.Service.IService
{
	public interface IAuthService
	{
		Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
		Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto);
		Task<ResponseDto> AssingRoleAsync(RegistrationRequestDto registrationRequestDto);
	}
}
