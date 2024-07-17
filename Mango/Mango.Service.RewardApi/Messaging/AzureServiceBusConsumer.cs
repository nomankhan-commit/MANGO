using Azure.Messaging.ServiceBus;
using Mango.Service.RewardApi.Message;
using Mango.Service.RewardApi.Model;
using Mango.Service.RewardApi.Model.Services;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Service.RewardApi.Messaging
{
    public class AzureServiceBusConsumer :IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _orderCreatedTopic;
        private readonly string _orderCreatedRewardSubscription;
        private readonly IConfiguration _configuration;
        private readonly RewardService _rewardService;
        private ServiceBusProcessor _rewardProcessor;
        private ServiceBusProcessor _registerUserProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration, RewardService rewardService)
        {
            _configuration = configuration;
            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            _orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueName:OrderCreatedTopic");
            _orderCreatedRewardSubscription = _configuration.GetValue<string>("TopicAndQueueName:OrderCreated_Reward_Subscription");

            var client = new ServiceBusClient(_serviceBusConnectionString);
            _rewardProcessor = client.CreateProcessor(_orderCreatedTopic, _orderCreatedRewardSubscription);
            _rewardService = rewardService;
        }

        public async Task Start()
        {
            _rewardProcessor.ProcessMessageAsync += OnNewOrderRequestReceived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardProcessor.StartProcessingAsync();

            _registerUserProcessor.ProcessMessageAsync += OnNewOrderRequestReceived;
            _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _registerUserProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            await _rewardProcessor.StopProcessingAsync();
            await _rewardProcessor.DisposeAsync();

            await _registerUserProcessor.StopProcessingAsync();
            await _registerUserProcessor.DisposeAsync();
        }
        private  Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        private async Task OnNewOrderRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            RewardMessage rewardMessage = JsonConvert.DeserializeObject<RewardMessage>(body);
            try
            {
               await _rewardService.UpDateReward(rewardMessage);
                args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
       
    }
}
