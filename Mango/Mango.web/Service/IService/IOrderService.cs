using Mango.web.Models.Dto;

namespace Mango.web.Service.IService
{
    public interface IOrderService
    {

        Task<ResponseDto?> CreateOrder(CartDto cartDto);
       
    }
}
