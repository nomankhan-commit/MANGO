using AutoMapper;
using Mango.MessageBus;
using Mango.Service.AuthAPI.Model.Dto;
using Mango.Service.AuthAPI.Service.IService;
using Mango.Services.AuthApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


//{
//	"userName": "nmk@gmail.com",
//  "password": "Admin123@"
//}

namespace Mango.Service.AuthAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthApiController : ControllerBase
	{

		private readonly  IAuthService _authService;
		protected ResponseDto _responseDto;
		private IMapper _mapper;
		private IMessageBus _messageBus;
		private IConfiguration _configuration;

		public AuthApiController(IAuthService authService, IMapper mapper,
			IMessageBus messageBus, IConfiguration configuration) { 
		
			_authService = authService;
			_mapper = mapper;
			_responseDto = new();
			_messageBus = messageBus;
            _configuration = configuration;

        }


		[HttpPost("register")]
		public async Task<IActionResult> Register(RegistrationRequestDto model) {

			string errormsg = await	_authService.Register(model);
			if (!string.IsNullOrEmpty(errormsg))
			{
				_responseDto.Sussess = false;
				_responseDto.Message = errormsg;
				return BadRequest(_responseDto);
			}
			_messageBus.PublishMessage(model.Email, "registeruser");
			//_messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicsAndQueueName:RegisterUserQueue"));
			return Ok(_responseDto);
		
		}

		[HttpPost("AssingRole")]
		public async Task<IActionResult> AssingRole(RegistrationRequestDto model)
		{

			bool assignRoleSuccessfully = await _authService.AssignRole(model.Email, model.Role.ToUpper());
			if (!assignRoleSuccessfully)
			{
				_responseDto.Sussess = false;
				_responseDto.Message = "Error encountered";
				return BadRequest(_responseDto);
			}
			return Ok(_responseDto);

		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginRequestDto model) {

			var loginresponse = await _authService.Login(model);
			if (loginresponse.User==null)
			{
				_responseDto.Sussess = false;
				_responseDto.Message = "Username or Password is incorrect.";
				return BadRequest(_responseDto);
			}
			_responseDto.Result = loginresponse;
			return Ok(_responseDto);
		
		}

	}
}
