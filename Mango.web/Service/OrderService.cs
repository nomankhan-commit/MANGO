using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using Mango.web.Utility;

namespace Mango.web.Service
{
    public class OrderService : IOrderService
    {
        private  readonly IBaseService _baseService;    
        public OrderService(IBaseService baseService) {

            _baseService = baseService;

        }

        public async Task<ResponseDto?> CreateOrder(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.POST,
                Data = cartDto,
                url = SD.OrderApiBase + "/api/Order/CreateOrder/"

            });
        }

        public async Task<ResponseDto?> GetAllOrders(string? userid)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.GET,
                url = SD.OrderApiBase + "/api/Order/GetOrders/" + userid

            });
        }

        public async Task<ResponseDto?> GetOrder(string? orderid)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.GET,
                url = SD.OrderApiBase + "/api/Order/GetOrder/"+orderid

            });
        }

        public async Task<ResponseDto?> UpdateOrderStatus(int orderid, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.POST,
                Data = newStatus,
                url = SD.OrderApiBase + "/api/Order/UpdateOrderStatus/"+orderid

            });
        }
    }
}
