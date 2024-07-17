using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(object message, string topic_queue_name);
        Task PublishMessageSpecificConnection(object message, string topic_queue_name, string service_bus_connection);
    }
}
