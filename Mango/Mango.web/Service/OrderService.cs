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
    }
}
