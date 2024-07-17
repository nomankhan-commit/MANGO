using Mango.web.Models.Dto;

namespace Mango.web.Service.IService
{
    public interface ICartService
    {

        Task<ResponseDto?> GetCartByUserIdAsync(string userid);
        Task<ResponseDto?> UpSertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);
        Task<ResponseDto?> EmailCart(CartDto cartDto);
       
       
    }
}
