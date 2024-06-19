using Mango.web.Model.Dto;
using Mango.web.Models;
using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel;


using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.web.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> List = new();
            ResponseDto? responseDto = await _productService.GetAllProductAsync();
            if (responseDto != null && responseDto.Sussess)
            {
                List = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }

            return View(List);
        }
        public async Task<IActionResult> Details(int productId)
        {
            ProductDto model = new();
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);
            if (responseDto != null && responseDto.Sussess)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            
            CartDto cartDto = new CartDto() 
            { 
                cartHeader = new CartHeaderDto()
                { 
                    UserId = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductID
            };    

            //List<CartDetailsDto> cartDetailsDtos = new List<CartDetailsDto>();
            List<CartDetailsDto> cartDetailsDtos = new() { cartDetails };
            cartDto.cartDetails = cartDetailsDtos;

            
            ResponseDto? responseDto = await _cartService.UpSertCartAsync(cartDto);
            if (responseDto != null && responseDto.Sussess)
            {
                TempData["success"] = "Items has been added to the shopping cart.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            return View(productDto);
        }



    }
}
