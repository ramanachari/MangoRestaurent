﻿using AutoMapper;
using Azure.Messaging.ServiceBus;
using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using Quartz;
using System.Text;

namespace Mango.Services.OrderAPI.Schedule
{
    [DisallowConcurrentExecution]
    public class CheckOutMessageReceivedTask : IJob
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private IMapper _mapper;

        public CheckOutMessageReceivedTask(IConfiguration configuration, IOrderRepository orderRepository ,IMapper mapper)
        {
            _configuration = configuration;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() => ReceiveMessageAsync());
            return task;
        }

        public async void ReceiveMessageAsync()
        {
            string serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            string subscriptionCheckOut = _configuration.GetValue<string>("SubscriptionCheckOut");
            string checkOutMessageTopic = _configuration.GetValue<string>("CheckOutMessageTopic");

            await using var client = new ServiceBusClient(serviceBusConnectionString);

            await using ServiceBusReceiver receiver = client.CreateReceiver(checkOutMessageTopic, subscriptionCheckOut);
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
            if (receivedMessage == null)
            {
                return;
            }
            var body = Encoding.UTF8.GetString(receivedMessage.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);
            OrderHeader orderHeader = _mapper.Map<OrderHeader>(checkoutHeaderDto);
            orderHeader.OrderDetails = _mapper.Map<List<OrderDetails>>(checkoutHeaderDto.CartDetails);
            orderHeader.CartTotalItems = orderHeader.OrderDetails.Sum(o => o.Count);

            await _orderRepository.AddOrderAsync(orderHeader);
            
        }
    }
}
