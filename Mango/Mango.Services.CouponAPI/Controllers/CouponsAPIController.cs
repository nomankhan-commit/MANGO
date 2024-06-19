using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/CouponApi")]
    [ApiController]
    [Authorize]
    public class CouponsAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDto _response;
        private IMapper _mapper;
        public CouponsAPIController(AppDbContext db, IMapper mapper) { 
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();  
        }

        [HttpGet]
        public object Get() {
            try
            {
                IEnumerable<Coupons> obj = _db.coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(obj);
            }
            catch (Exception ex)
            {
                _response.Sussess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpGet]
        [Route("{id:int}")]
        public object Get(int? id)
        {
            try
            {

                Coupons obj = _db.coupons.First(x => x.CouponID == id);
                _response.Result = _mapper.Map<CouponDto>(obj);
                //_response.Result = _mapper.Map<Coupons,CouponDto>(obj);

            }
            catch (Exception ex)
            {
                _response.Sussess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpGet]
        [Route("GetByCode/{code}")]
        public object GetByCode(string code)
        {
            try
            {

                Coupons obj = _db.coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
                if (obj==null)
                {
                    _response.Sussess = false;
                }
                _response.Result = _mapper.Map<CouponDto>(obj);
                //_response.Result = _mapper.Map<Coupons,CouponDto>(obj);

            }
            catch (Exception ex)
            {
                _response.Sussess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        //[Route("CreateCoupon")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post(CouponDto couponDto) {

            try
            {
                Coupons coupons = _mapper.Map<Coupons>(couponDto);
                _db.coupons.Add(coupons);
                _db.SaveChanges();
                _response.Result = couponDto;
            }
            catch (Exception ex)
            {

                _response.Sussess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }


        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {

            try
            {
                Coupons coupons = _mapper.Map<Coupons>(couponDto);
                _db.coupons.Update(coupons);
                _db.SaveChanges();
                _response.Result = couponDto;
            }
            catch (Exception ex)
            {

                _response.Sussess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }


        [HttpDelete]
		[Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {

            try
            {
                Coupons obj = _db.coupons.First(x => x.CouponID == id);
                _db.coupons.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

                _response.Sussess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

    }
}
