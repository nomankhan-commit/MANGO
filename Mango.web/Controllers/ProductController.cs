using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.web.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService _productService;
        public ProductController(IProductService productService) {

            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {

            List<ProductDto> List = new();
            ResponseDto? responseDto = await _productService.GetAllProductAsync();
            if (responseDto!=null && responseDto.Sussess)
            {
                List = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            
            return View(List);
        }


		public async Task<IActionResult> ProductCreate()
		{

			return View();
		}

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
		{

            if (ModelState.IsValid)
            {
                List<ProductDto> List = new();
                ResponseDto? responseDto = await _productService.CreateProductAsync(model);
                if (responseDto != null && responseDto.Sussess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = responseDto.Message;
                }
            }
          
            return View(model);
        }



        
        public async Task<IActionResult> ProductDelete(int productid)
        {

            ProductDto Product = new();
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productid);
            if (responseDto != null && responseDto.Sussess)
            {
                Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
            }


            return View(Product);
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto model)
        {

            ProductDto Product = new();
            ResponseDto? responseDto = await _productService.DeleteProductAsync(model.ProductID);
            if (responseDto != null && responseDto.Sussess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View(model);
            }
        }

    }
}
