using Mango.web.Models.Dto;

namespace Mango.web.Service.IService
{
    public interface ICouponService
    {

        Task<ResponseDto?> GetCouponAsync(string coupon);
        Task<ResponseDto?> GetAllCouponAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(CouponDto coupon);
        Task<ResponseDto?> UpdateCouponAsync(CouponDto coupon);
        Task<ResponseDto?> DeleteCouponAsync(int id);
       
    }
}
