using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using Mango.web.Utility;

namespace Mango.web.Service
{
    public class ProductService : IProductService
    {
        private  readonly IBaseService _baseService;    
        public ProductService(IBaseService baseService) {

            _baseService = baseService;

        }

        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.POST,
                Data = productDto,
                url = SD.ProductApiBase + "/api/ProductApi/"

            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.DELETE,
                url = SD.ProductApiBase + "/api/ProductApi/" + id

            });
        }

        public async Task<ResponseDto?> GetAllProductAsync()
        {

            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.GET,
                url = SD.ProductApiBase + "/api/ProductApi"

            }); 

     
        }

        public async Task<ResponseDto?> GetProductAsync(string product)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.GET,
                url = SD.ProductApiBase + "/api/ProductApi/GetByCode" + product

            });
        }

        public Task<ResponseDto?> GetProductAsync(int productid)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.GET,
                url = SD.ProductApiBase + "/api/ProductApi/" + id

            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.PUT,
                Data = productDto,
                url = SD.ProductApiBase + "/api/ProductApi/"

            });
        }
    }
}
