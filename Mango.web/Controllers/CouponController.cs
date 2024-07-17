using Mango.web.Models.Dto;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.web.Controllers
{
    public class CouponController : Controller
    {

        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService) { 
         
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            
            List<CouponDto> List = new();
            ResponseDto? responseDto = await _couponService.GetAllCouponAsync();
            if (responseDto!=null && responseDto.Sussess)
            {
                List = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            
            return View(List);
        }


		public async Task<IActionResult> CouponCreate()
		{

			return View();
		}

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
		{

            if (ModelState.IsValid)
            {
                List<CouponDto> List = new();
                ResponseDto? responseDto = await _couponService.CreateCouponAsync(model);
                if (responseDto != null && responseDto.Sussess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = responseDto.Message;
                }
            }
          
            return View(model);
        }



        
        public async Task<IActionResult> CouponDelete(int couponId)
        {

            CouponDto coupon = new();
            ResponseDto? responseDto = await _couponService.GetCouponByIdAsync(couponId);
            if (responseDto != null && responseDto.Sussess)
            {
                coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
            }


            return View(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto model)
        {

            CouponDto coupon = new();
            ResponseDto? responseDto = await _couponService.DeleteCouponAsync(model.CouponID);
            if (responseDto != null && responseDto.Sussess)
            {
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View(model);
            }
        }

    }
}
