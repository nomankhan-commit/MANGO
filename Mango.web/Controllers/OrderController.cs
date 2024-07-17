using Mango.web.Model.Dto;
using Mango.web.Models.Dto;
using Mango.web.Service;
using Mango.web.Service.IService;
using Mango.web.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.web.Controllers
{
    public class OrderController : Controller
    {
        IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
                 _orderService = orderService;
        }

        public IActionResult Index()
        {

            IEnumerable<OrderHeaderDto> list = new List<OrderHeaderDto>();
            string userid = "";
            if (!User.IsInRole(SD.RoleAdmin))
            {
                userid = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }
            ResponseDto responseDto = _orderService.GetAllOrders(userid).GetAwaiter().GetResult();
            if (responseDto != null && responseDto.Sussess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(responseDto.Result));
            }


            return View(list);
        }

        [HttpGet]
        public IActionResult Getall()
        {
            IEnumerable<OrderHeaderDto> list  = new List<OrderHeaderDto>();
            string userid = "";
            if (!User.IsInRole(SD.RoleAdmin))
            {
                userid = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value; 
            }
            ResponseDto responseDto = _orderService.GetAllOrders(userid).GetAwaiter().GetResult();
            if (responseDto!=null  && responseDto.Sussess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(responseDto.Result));
            }
            return Json(new {data = list });
        }
    }
}
