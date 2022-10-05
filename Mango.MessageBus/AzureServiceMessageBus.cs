using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Mango.MessageBus
{
    public class AzureServiceMessageBus : IMessageBus
    {
        public async Task PublishMessageAsync(BaseMessage message, string topicName, string connectionString)
        {
            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(topicName);

            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);

            await sender.DisposeAsync();

        }
    }
}
