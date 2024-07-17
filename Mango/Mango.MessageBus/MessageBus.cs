using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private string ConnectionString = "Endpoint=sb://mangoserver1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=3htbjMDlaJj8shC195qXriaobI81Okyyz+ASbHSLj74=";
        public async Task PublishMessage(object message, string topic_queue_name)
        {
            await using var client = new ServiceBusClient(ConnectionString);
            ServiceBusSender sender =  client.CreateSender(topic_queue_name);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage
                (Encoding.UTF8.GetBytes(jsonMessage))
            { 
             CorrelationId = Guid.NewGuid().ToString(),
            };

            await  sender.SendMessageAsync(serviceBusMessage);
            await sender.DisposeAsync();

        }
        public async Task PublishMessageSpecificConnection(object message, string topic_queue_name, string service_bus_connection)
        {
            await using var client = new ServiceBusClient(service_bus_connection);
            ServiceBusSender sender =  client.CreateSender(topic_queue_name);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage
                (Encoding.UTF8.GetBytes(jsonMessage))
            { 
             CorrelationId = Guid.NewGuid().ToString(),
            };

            await  sender.SendMessageAsync(serviceBusMessage);
            await sender.DisposeAsync();

        }
    }
}
