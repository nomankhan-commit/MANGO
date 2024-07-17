using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Mango.Service.ShoppingCart.Model.Dto;
using AutoMapper;
using Mango.Service.ShoppingCart.Data;
using Microsoft.EntityFrameworkCore;
using Mango.Service.ShoppingCart.Model;
using System.Reflection.PortableExecutable;
using Mango.Service.ShoppingCart.Service.IService;
using Mango.Service.ShoppingCart.Models.Dto;
using Azure;
using Mango.MessageBus;

namespace Mango.Service.ShoppingCart.Controllers
{
    [Route("api/cartapi")]
    [ApiController]

    public class ShoppingCartApiController : ControllerBase
    {
       
        private readonly ResponseDto _responseDto;
        private IMapper _mapper;
        private IProductService _productService;
        private ICouponService _couponService;
        private readonly AppDbContext _appDbContext;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;

        public ShoppingCartApiController(AppDbContext db, IMapper mapper,
            IProductService productService, ICouponService couponService, IMessageBus messageBus,
            IConfiguration configuration
            )
        {
            _appDbContext = db;
            _mapper = mapper;
            _productService = productService;
            _couponService = couponService;
            _responseDto = new ResponseDto();
            _messageBus = messageBus;
            _configuration = configuration;
            

        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userid) {
            try
            {
                CartDto cart = new() { 
                cartHeader = _mapper.Map<CartHeaderDto>(_appDbContext.CartHeader.FirstOrDefault(x=>x.UserId==userid))
                };
                cart.cartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_appDbContext.
                    CartDetails.Where(x => x.CartHeaderId == cart.cartHeader.CartHeaderId));
                IEnumerable<ProductDto> productDtos = await _productService.GetProduct();
                
                foreach (var item in cart.cartDetails)
                { 
                    item.ProductDto = productDtos.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cart.cartHeader.CartTotal += (item.Count * item.ProductDto.Price);
                }

                //apply coupon if any
                if (!string.IsNullOrEmpty(cart.cartHeader.CouponCode))
                {
                    CouponDto couponDto = await _couponService.GetCoupon(cart.cartHeader.CouponCode);
                    if (couponDto!=null && cart.cartHeader.CartTotal > couponDto.MinAmount)
                    {
                        cart.cartHeader.CartTotal -= couponDto.DiscountAmount;
                        cart.cartHeader.Discount = couponDto.DiscountAmount;

                    }
                }

                _responseDto.Result = cart;
                _responseDto.Sussess = true;
            }
            catch (Exception ex)
            {

                _responseDto.Sussess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon(CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _appDbContext.CartHeader.FirstAsync(o=>o.UserId == cartDto.cartHeader.UserId);
                cartFromDb.CouponCode = cartDto.cartHeader.CouponCode;
                _appDbContext.CartHeader.Update(cartFromDb);
               await _appDbContext.SaveChangesAsync();   
                _responseDto.Result = true;
                _responseDto.Sussess = true;
            }
            catch (Exception ex)
            {

                _responseDto.Sussess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon(CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _appDbContext.CartHeader.FirstAsync(o => o.UserId == cartDto.cartHeader.UserId);
                cartFromDb.CouponCode = "";
                _appDbContext.CartHeader.Update(cartFromDb);
                await _appDbContext.SaveChangesAsync();
                _responseDto.Result = true;
                _responseDto.Sussess = true;
            }
            catch (Exception ex)
            {

                _responseDto.Sussess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto) {
            try
            {
                var cartHeaderFromDb = await _appDbContext.CartHeader.FirstOrDefaultAsync(x=>x.UserId == cartDto.cartHeader.UserId);
                if (cartHeaderFromDb == null) {
                
                    // cart header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.cartHeader);
                    _appDbContext.CartHeader.Add(cartHeader);
                    await _appDbContext.SaveChangesAsync();
                    
                    cartDto.cartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.cartDetails.First());
                    _appDbContext.CartDetails.Add(cartDetails);
                    await _appDbContext.SaveChangesAsync();
                }
                else {

                    //if header is not null.
                    //check if details has some product.
                    var cartDetailsFromDb = await _appDbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(x=>x.ProductId 
                    == cartDto.cartDetails.First().ProductId 
                    && x.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb==null)
                    {
                        // create card details.
                        cartDto.cartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.cartDetails.First());
                        _appDbContext.CartDetails.Add(cartDetails);
                        await _appDbContext.SaveChangesAsync();

                    }
                    else
                    {
                        // update cartdetails.
                        cartDto.cartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.cartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.cartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _appDbContext.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.cartDetails.First()));
                        await _appDbContext.SaveChangesAsync();
                    }
                }
                _responseDto.Result = cartDto;
            }
            catch (Exception ex)
            {

                _responseDto.Sussess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {

                CartDetails cartDetails1 = _appDbContext.CartDetails.First(x => x.CartDetailsId == cartDetailsId);
                var totalCountofCartItems = _appDbContext.CartDetails.Where(x => x.CartHeaderId == cartDetails1.CartHeaderId).Count();
                _appDbContext.CartDetails.Remove(cartDetails1);
                if (totalCountofCartItems == 1)
                {
                    var cartHeaderToRemove = await _appDbContext.CartHeader.FirstOrDefaultAsync(x=>x.CartHeaderId == cartDetails1.CartHeaderId);
                    _appDbContext.CartHeader.Remove(cartHeaderToRemove);
                }
                await _appDbContext.SaveChangesAsync();
                _responseDto.Sussess = true;
                _responseDto.Result = null;
            }
            catch (Exception ex)
            {

                _responseDto.Sussess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }


        [HttpPost("EmailCartRequest")]
        public async Task<ResponseDto> EmailCartRequest(CartDto cartDto)
        {
            try
            {
                await _messageBus.PublishMessage(cartDto, _configuration.GetValue<string>("TopicsAndQueueName:EmailShoppingCart"));
                _responseDto.Result = true;
                _responseDto.Sussess = true;
            }
            catch (Exception ex)
            {

                _responseDto.Sussess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

    }
}
