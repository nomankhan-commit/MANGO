using Mango.Service.ShoppingCart.Model.Dto;
using Mango.Service.ShoppingCart.Models.Dto;
using Mango.Service.ShoppingCart.Service.IService;
using Newtonsoft.Json;

namespace Mango.Service.ShoppingCart.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
                _httpClientFactory = httpClientFactory; 
        }

        

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/CouponApi/GetByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp != null && resp.Sussess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(
                    Convert.ToString(resp.Result));
            }
            return new CouponDto();
        }


    }
}
