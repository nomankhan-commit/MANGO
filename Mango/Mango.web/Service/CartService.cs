using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using Mango.web.Utility;

namespace Mango.web.Service
{
    public class CartService : ICartService
    {
        private  readonly IBaseService _baseService;    
        public CartService(IBaseService baseService) {

            _baseService = baseService;

        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.POST,
                Data = cartDto,
                url = SD.ShoppingCartApiBase + "/api/cartapi/ApplyCoupon/"

            });
        }

        public async Task<ResponseDto?> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.POST,
                Data = cartDto,
                url = SD.ShoppingCartApiBase + "/api/cartapi/EmailCartRequest/"

            });
        }

        public async Task<ResponseDto?> GetCartByUserIdAsync(string userid)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.GET,
                url = SD.ShoppingCartApiBase + "/api/cartapi/GetCart/" + userid

            });
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.POST,
                Data = cartDetailsId,
                url = SD.ShoppingCartApiBase + "/api/cartapi/RemoveCart/"

            });
        }

        public async Task<ResponseDto?> UpSertCartAsync(CartDto cartDto)
        {

            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.POST,
                Data = cartDto,
                url = SD.ShoppingCartApiBase + "/api/cartapi/CartUpsert/"

            });
        }
    }
}
