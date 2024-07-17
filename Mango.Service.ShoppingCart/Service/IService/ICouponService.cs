using Mango.Service.ShoppingCart.Models.Dto;

namespace Mango.Service.ShoppingCart.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
        //Task<IEnumerable<CouponDto>> GetCoupon(string couponCode);
    }
}
