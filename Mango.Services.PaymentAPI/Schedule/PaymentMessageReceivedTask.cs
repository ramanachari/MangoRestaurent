using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.PaymentProcessor;
using Mango.Services.PaymentAPI.Messages;
using Newtonsoft.Json;
using Quartz;
using System.Text;

namespace Mango.Services.PaymentAPI.Schedule
{
    [DisallowConcurrentExecution]
    public class PaymentMessageReceivedTask : IJob
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;
        private readonly IProcessPayment _processPayment;

        public PaymentMessageReceivedTask(IConfiguration configuration, IMessageBus messageBus, IProcessPayment processPayment)
        {
            _configuration = configuration;
            _messageBus = messageBus;
            _processPayment = processPayment;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() => ReceivePaymentMessageAsync());
            return task;
        }

        public async void ReceivePaymentMessageAsync()
        {
            string serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            string subscriptionMangoPayment = _configuration.GetValue<string>("SubscriptionMangoPayment");
            string orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopic");

            await using var client = new ServiceBusClient(serviceBusConnectionString);

            await using ServiceBusReceiver receiver = client.CreateReceiver(orderPaymentProcessTopic, subscriptionMangoPayment);
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
            if (receivedMessage == null)
            {
                return;
            }
            var body = Encoding.UTF8.GetString(receivedMessage.Body);

            PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            var result = _processPayment.PaymentProcessor();

            UpdatePaymentResultMessage updatePaymentResultMessage = new()
            {
                OrderId = paymentRequestMessage.OrderId,
                Status = result
            };

            string orderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");
            try
            {
                await _messageBus.PublishMessageAsync(updatePaymentResultMessage, orderUpdatePaymentResultTopic, serviceBusConnectionString);
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
