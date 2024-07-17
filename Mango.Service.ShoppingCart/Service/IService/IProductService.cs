using Mango.Service.ShoppingCart.Model.Dto;

namespace Mango.Service.ShoppingCart.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProduct();
    }
}
