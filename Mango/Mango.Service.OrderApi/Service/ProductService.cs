using Mango.Service.OrderApi.Model.Dto;
using Mango.Service.OrderApi.Models.Dto;
using Mango.Service.OrderApi.Service.IService;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Mango.Service.OrderApi.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ResponseDto _responseDto;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _responseDto = new ResponseDto();

        }
        public async Task<IEnumerable<ProductDto>> GetProduct()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/ProductAPI");
            var apiContent = await response.Content.ReadAsStringAsync();
           var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (responseDto.Sussess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(
                    Convert.ToString(responseDto.Result));
            }
            return new List<ProductDto>();
        }
    }
}
