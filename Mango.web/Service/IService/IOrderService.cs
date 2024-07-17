using Mango.web.Models.Dto;

namespace Mango.web.Service.IService
{
    public interface IOrderService
    {

        Task<ResponseDto?> CreateOrder(CartDto cartDto);
        Task<ResponseDto?> GetAllOrders(string? userid);
        Task<ResponseDto?> GetOrder(string? orderid);
        Task<ResponseDto?> UpdateOrderStatus(int orderid, string newStatus);
       
    }
}
