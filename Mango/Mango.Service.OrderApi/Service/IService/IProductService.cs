using Mango.Service.OrderApi.Model.Dto;
using Mango.Service.OrderApi.Models.Dto;

namespace Mango.Service.OrderApi.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProduct();
    }
}
