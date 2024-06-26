﻿using Mango.web.Models.Dto;
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
        public CartController(ICartService cartService)
        {
            _cartService = cartService;  
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await LoadCartDtoBasedonLoggedInUser());
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
