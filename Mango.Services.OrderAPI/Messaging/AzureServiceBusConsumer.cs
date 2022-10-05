using AutoMapper;
using Azure.Messaging.ServiceBus;
using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor checkOutProcessor;
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionCheckOut;
        private readonly string checkOutMessageTopic;

        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            //_orderRepository = orderRepository;
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionCheckOut = _configuration.GetValue<string>("SubscriptionCheckOut");
            checkOutMessageTopic = _configuration.GetValue<string>("CheckOutMessageTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);
            checkOutProcessor = client.CreateProcessor(checkOutMessageTopic);
        }

        public async Task Start()
        {
            checkOutProcessor.ProcessMessageAsync += OnCheckOutMessageReceived;
            checkOutProcessor.ProcessErrorAsync += OnCheckOutErrorHandler;
            await checkOutProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await checkOutProcessor.StopProcessingAsync();
            await checkOutProcessor.DisposeAsync();
        }

        private Task OnCheckOutErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnCheckOutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            //OrderHeader orderHeader = _mapper.Map<OrderHeader>(checkoutHeaderDto);

            //OrderHeader orderHeader = new()
            //{
            //    CardNumber = checkoutHeaderDto.CardNumber,
            //    CouponCode = checkoutHeaderDto.CouponCode,
            //    CVV = checkoutHeaderDto.CVV,
            //    DiscountAmount = checkoutHeaderDto.DiscountAmount,
            //    Email = checkoutHeaderDto.Email,
            //    ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
            //    FirstName = checkoutHeaderDto.FirstName,
            //    LastName = checkoutHeaderDto.LastName,
            //    OrderDetails = new List<OrderDetails>(),
            //    OrderTotal = checkoutHeaderDto.OrderTotal,
            //    PaymentStatus = false,
            //    UserId = checkoutHeaderDto.UserId,
            //    Phone = checkoutHeaderDto.Phone,
            //    PickUpDateTime = checkoutHeaderDto.PickUpDateTime,
            //};

            //orderHeader.OrderTime = DateTime.Now;
            //orderHeader.CartTotalItems = checkoutHeaderDto.CartDetails.Sum(c => c.Count);

           //await _orderRepository.AddOrderAsync(orderHeader);

        }
    }
}
