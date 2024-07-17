using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Newtonsoft;
using Newtonsoft.Json;
using System.Text;
using static Mango.web.Utility.SD;
using System.Net;
namespace Mango.web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) {
        
            _httpClientFactory = httpClientFactory; 
            _tokenProvider = tokenProvider;

        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {

            try
            {

            }
            catch (Exception ex)
            {
                var dto = new ResponseDto { Sussess = false, Message = ex.Message.ToString() };
                return dto;
            }


            HttpClient Client = _httpClientFactory.CreateClient("Mango");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            //token
            if (withBearer)
            {
               string token = _tokenProvider.GetToken();
                message.Headers.Add("Authorization",$"Bearer {token}");
            }

            message.RequestUri = new Uri(requestDto.url);
            if (requestDto.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data),Encoding.UTF8,"application/json");
            }

            HttpResponseMessage? apiresponse = null;

            switch (requestDto.ApiType)
            {
                case ApiType.GET:
                    message.Method = HttpMethod.Get;
                    break;
                case ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    break;
            }

            apiresponse = await Client.SendAsync(message);

            switch (apiresponse.StatusCode)
            {

                case HttpStatusCode.Forbidden:
                    return new() { Sussess = false, Message = "Access Denied" };
                case HttpStatusCode.MethodNotAllowed:
                    return new() { Sussess = false, Message = "Method Not Allowed" };
                case HttpStatusCode.NotFound:
                    return new() { Sussess = false, Message = "Not Found" };
                case HttpStatusCode.Unauthorized:
                    return new() { Sussess = false, Message = "Unauthorized" };
                case HttpStatusCode.InternalServerError:
                    return new() { Sussess = false, Message = "Internal Server Error" };
                case HttpStatusCode.BadRequest:
                    return new() { Sussess = false, Message = "BadRequest" };
                default:
                    var apiContent = await apiresponse.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return apiResponseDto;
            }
        }
    }
}
