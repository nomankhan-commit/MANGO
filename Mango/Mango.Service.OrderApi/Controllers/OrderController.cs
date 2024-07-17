using AutoMapper;
using Mango.MessageBus;
using Mango.Service.OrderApi.Data;
using Mango.Service.OrderApi.Model;
using Mango.Service.OrderApi.Model.Dto;
using Mango.Service.OrderApi.Model.Utility;
using Mango.Service.OrderApi.Models.Dto;
using Mango.Service.OrderApi.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;

namespace Mango.Service.OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        protected ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private IProductService _productService;
        private IMessageBus _messageBus;
        private IConfiguration _configuration;
        public OrderController(AppDbContext appDbContext,
            IProductService productService,IMapper mapper,
            IMessageBus messageBus, IConfiguration configuration)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;   
            _productService = productService;
            _messageBus = messageBus;
            _configuration = configuration;
            _response = new ResponseDto();
        }

        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto) {

            try
            {
                //[FromBody]
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.cartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.cartDetails);

                //var t = _mapper.Map<OrderHeader>(cartDto.cartHeader);

                var g = _mapper.Map<OrderHeader>(orderHeaderDto);

                OrderHeader orderCreated =  _appDbContext.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _appDbContext.SaveChangesAsync();
                RewardDto reward = new() {
                    OrderId = orderCreated.OrderHeaderId,
                    RewardActivity = Convert.ToInt32(orderCreated.OrderTotal),
                    UserId = orderCreated.UserId
                };
               string _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
                string topicname = _configuration.GetValue<string>("TopicsAndQueueName:OrderCreatedTopic");
               //await _messageBus.PublishMessage(reward, topicname);
               await _messageBus.PublishMessageSpecificConnection(reward, topicname, _serviceBusConnectionString);
                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDto;
            }
            catch (Exception ex)
            {
                _response.Result = ex.Message;
                _response.Sussess = false;
            }
            return _response;
        }

    }
}
