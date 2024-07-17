using Azure.Messaging.ServiceBus;
using Mango.Service.EmailApi.Message;
using Mango.Service.EmailApi.Model.Dto;
using Mango.Service.EmailApi.Model.Services;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Service.EmailApi.Data.Messaging
{
    public class AzureServiceBusConsumer :IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _serviceBusOrderConnectionString;
        private readonly string _emailCartQueue;
        private readonly string _registerUserQueue;
        
        private readonly string _orderCreated_Topic;
        private readonly string _orderCreated_Email_Subscription;
        private ServiceBusProcessor _emailOrderPlacedProcessor;

        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _registerUserProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            _serviceBusOrderConnectionString = _configuration.GetValue<string>("ServiceBusOrderConnectionString");
            _emailCartQueue = _configuration.GetValue<string>("TopicAndQueueName:EmailShoppingCart");
            _orderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueName:OrderCreatedTopic");
            _orderCreated_Email_Subscription = _configuration.GetValue<string>("TopicAndQueueName:OrderCreated_Reward_Subscription");
            
            var client = new ServiceBusClient(_serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(_emailCartQueue);

            _registerUserQueue = _configuration.GetValue<string>("TopicAndQueueName:RegisterUserQueue");
            _registerUserProcessor = client.CreateProcessor(_registerUserQueue);

            var client1 = new ServiceBusClient(_serviceBusOrderConnectionString);
            _emailOrderPlacedProcessor = client1.CreateProcessor(_orderCreated_Topic, _orderCreated_Email_Subscription);
            
            _emailService = emailService;
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _registerUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
            _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _registerUserProcessor.StartProcessingAsync();

            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _registerUserProcessor.StopProcessingAsync();
            await _registerUserProcessor.DisposeAsync();
        }
        private  Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
               await _emailService.EmailCartLog(cartDto);
                args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                await _emailService.RegisterUserEmailandLog(email);
                args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            RewardMessage rewardMessage = JsonConvert.DeserializeObject<RewardMessage>(body);
            try
            {
                await _emailService.LogPlacedOrder(rewardMessage);
                args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
