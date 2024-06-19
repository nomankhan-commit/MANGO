using AutoMapper;
using Mango.Service.ProductApi.Data;
using Mango.Service.ProductApi.Model;
using Mango.Service.ProductApi.Model.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.ProductApi.Controllers
{
    [Route("api/ProductAPI")]
    [ApiController]
    //[Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDto _response;
        private IMapper _mapper;
        public ProductAPIController(AppDbContext db, IMapper mapper) { 
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();  
        }

        [HttpGet]
        public object Get() {
            try
            {
                IEnumerable<Product> obj = _db.product.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(obj);
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

                Product obj = _db.product.First(x => x.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(obj);
                //_response.Result = _mapper.Map<Product,CouponDto>(obj);

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
        public ResponseDto Post(ProductDto productDto) {

            try
            {
                Product Product = _mapper.Map<Product>(productDto);
                _db.product.Add(Product);
                _db.SaveChanges();
                _response.Result = productDto;
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
        public ResponseDto Put([FromBody] ProductDto productDto)
        {

            try
            {
                Product Product = _mapper.Map<Product>(productDto);
                _db.product.Update(Product);
                _db.SaveChanges();
                _response.Result = productDto;
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
                Product obj = _db.product.First(x => x.ProductId == id);
                _db.product.Remove(obj);
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
