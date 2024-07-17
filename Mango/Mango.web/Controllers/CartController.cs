using Mango.web.Model.Dto;
using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.web.Controllers
{
    public class CartController : Controller
    {

        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;  
            _orderService = orderService;
         
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await LoadCartDtoBasedonLoggedInUser());
        }
        
        public async Task<IActionResult> checkout()
        {
            return View(await LoadCartDtoBasedonLoggedInUser());
        }

        [Authorize]
        public async Task<IActionResult> Confirm(int orderid)
        {
            return View(orderid);
        }

        [HttpPost]
        [ActionName("checkout")]
        public async Task<IActionResult> checkout(CartDto cartDto)
        {
           CartDto cart=  await LoadCartDtoBasedonLoggedInUser();
            cart.cartHeader.PhoneNumber = cartDto.cartHeader.PhoneNumber;
            cart.cartHeader.Email = cartDto.cartHeader.Email;
            cart.cartHeader.Name = cartDto.cartHeader.Name;

          var response = await _orderService.CreateOrder(cart);
            OrderHeaderDto orderHeaderDto = 
                JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
            if (response != null && response.Sussess)
            {

            }
            return View(cart);    
        }

        public async Task<IActionResult> Remove(int cartDetailId)
        {
            //var userid = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? responseDto = await _cartService.RemoveFromCartAsync(cartDetailId);
            if (responseDto != null && responseDto.Sussess)
            {
                TempData["success"] = "Cart updated successfully.";
            }
            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var userid = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? responseDto = await _cartService.ApplyCouponAsync(cartDto);
            if (responseDto != null && responseDto.Sussess)
            {
                TempData["success"] = "Cart updated successfully.";
            }
            return RedirectToAction(nameof(Index));

        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {

            cartDto.cartHeader.CouponCode = "";
            var userid = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? responseDto = await _cartService.ApplyCouponAsync(cartDto);
            if (responseDto != null && responseDto.Sussess)
            {
                TempData["success"] = "Cart updated successfully.";
            }
            return RedirectToAction(nameof(Index));

        }

        public async Task<CartDto>  LoadCartDtoBasedonLoggedInUser() {

        var userid = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        ResponseDto? responseDto =  await _cartService.GetCartByUserIdAsync(userid);
            if (responseDto !=null && responseDto.Sussess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(responseDto.Result));
                return cartDto;
            }
        return new CartDto();
        }


        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBasedonLoggedInUser();
            cart.cartHeader.Email = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
            ResponseDto? responseDto = await _cartService.EmailCart(cart);
            if (responseDto != null && responseDto.Sussess)
            {
                TempData["success"] = "Cart send to your email successfully.";
            }
            return RedirectToAction(nameof(Index));

        }

    }
}
