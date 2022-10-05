using Mango.Services.OrderAPI.Messaging;
using System.Reflection.Metadata;

namespace Mango.Services.OrderAPI.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }

        //public static IApplicationBuilder UserAzureServiceBusConsumer(this IApplicationBuilder applicationBuilder, IServiceCollection serviceCollection)
        public static IApplicationBuilder UserAzureServiceBusConsumer(this IApplicationBuilder applicationBuilder)
        {
            //ServiceBusConsumer = serviceCollection.BuildServiceProvider().GetRequiredService<IAzureServiceBusConsumer>();
            ServiceBusConsumer = applicationBuilder.ApplicationServices.GetService<IAzureServiceBusConsumer>();

            var hostApplicationLife = applicationBuilder.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStarted.Register(OnStop);

            return applicationBuilder;
        }

        private static void OnStart()
        { 
            ServiceBusConsumer.Start();
        }

        private static void OnStop()
        {
            ServiceBusConsumer.Stop();
        }
    }
}
