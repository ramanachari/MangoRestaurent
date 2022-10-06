using AutoMapper;
using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using Quartz;
using System.Text;

namespace Mango.Services.OrderAPI.Schedule
{
    [DisallowConcurrentExecution]
    public class OrderPaymentUpdateResultTask : IJob
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;

        public OrderPaymentUpdateResultTask(IConfiguration configuration, IOrderRepository orderRepository)
        {
            _configuration = configuration;
            _orderRepository = orderRepository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() => ReceivePaymentMessageAsync());
            return task;
        }

        public async void ReceivePaymentMessageAsync()
        {
            string serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            string subscriptionpaymentUpdate = _configuration.GetValue<string>("SubscriptionCheckOut");
            string paymentUpdateMessageTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");

            await using var client = new ServiceBusClient(serviceBusConnectionString);

            await using ServiceBusReceiver receiver = client.CreateReceiver(paymentUpdateMessageTopic, subscriptionpaymentUpdate);
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
            if (receivedMessage == null)
            {
                return;
            }
            var body = Encoding.UTF8.GetString(receivedMessage.Body);

            UpdatePaymentResultMessage orderPaymentResult = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            await _orderRepository.UpdateOrderPaymentStatusAsync(orderPaymentResult.OrderId, orderPaymentResult.Status);

        }
    }
}
