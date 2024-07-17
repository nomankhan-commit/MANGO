using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using Mango.web.Utility;

namespace Mango.web.Service
{
    public class CouponService : ICouponService
    {
        private  readonly IBaseService _baseService;    
        public CouponService(IBaseService baseService) {

            _baseService = baseService;

        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.POST,
                Data = couponDto,
                url = SD.CouponApiBase + "/api/CouponApi/"

            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.DELETE,
                url = SD.CouponApiBase + "/api/CouponApi/" + id

            });
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {

            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.GET,
                url = SD.CouponApiBase + "/api/CouponApi"

			}); 

     
        }

        public async Task<ResponseDto?> GetCouponAsync(string coupon)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.GET,
                url = SD.CouponApiBase + "/api/CouponApi/GetByCode" + coupon

            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.GET,
                url = SD.CouponApiBase + "/api/CouponApi/" + id

            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {

                ApiType = SD.ApiType.PUT,
                Data = couponDto,
                url = SD.CouponApiBase + "/api/CouponApi/"

			});
        }
    }
}
