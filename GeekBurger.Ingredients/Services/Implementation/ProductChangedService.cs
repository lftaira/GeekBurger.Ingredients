using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeekBurger.Ingredients.Services.Interface;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace GeekBurger.Ingredients.Services.Implementation
{
    public class ProductChangedService : IProductChangedService
    {

        private const string TopicName = "ProductChangedTopic";
        private const string SubscriptionName = "ingredients";
        private readonly IConfiguration configuration;
        private readonly IServiceProvider serviceProvider;
        private readonly IServiceBusNamespace serviceBusNamespace;
        public ProductChangedService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;
            this.serviceBusNamespace = configuration.GetServiceBusNamespace();
        }

        public async void ReceiveMessages()
        {

            CreateSub();
            var config = this.configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            var _queueClient = new SubscriptionClient(config.ConnectionString, TopicName, SubscriptionName);
            _queueClient.RegisterMessageHandler(MessageHandler,new MessageHandlerOptions(ExceptionHandler) { AutoComplete = false });
        }
        private static Task MessageHandler(Message message, CancellationToken arg2)
        {
            Console.WriteLine($"message Label: {message.Label}");
            Console.WriteLine($"CorrelationId: {message.CorrelationId}");
            var productChanged = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("Message Received");
            Console.WriteLine(productChanged);

            //Thread.Sleep(40000);

            return Task.CompletedTask;
        }

        private static Task ExceptionHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Handler exception {arg.Exception}.");
            var context = arg.ExceptionReceivedContext;
            Console.WriteLine($"Endpoint: {context.Endpoint}, Path: { context.EntityPath}, Action: { context.Action}");
            return Task.CompletedTask;
        }


        private void CreateSub()
        {
            if (!serviceBusNamespace.Topics.List()
               .Any(t => t.Name
               .Equals(TopicName, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            var topic = serviceBusNamespace.Topics.GetByName(TopicName);

            if (topic.Subscriptions.List()
              .Any(subscription => subscription.Name
              .Equals(SubscriptionName,
                     StringComparison.InvariantCultureIgnoreCase)))
            {
                topic.Subscriptions.DeleteByName(SubscriptionName);
            }

            topic.Subscriptions
                .Define(SubscriptionName)
                .Create();
        }
    }
}