using System;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using GeekBurger.Ingredients.Services.Interface;

namespace GeekBurger.Ingredients.Services.Implementation
{
    public class LabelImageAddedService : ILabelImaggeAddedService
    {
        private const string TopicName = "LabelImageAdded";
        private const string SubscriptionName = "ingredientsub";

        private readonly IConfiguration configuration;
        private readonly IServiceProvider serviceProvider;
        private readonly IServiceBusNamespace serviceBusNamespace;
        private readonly bool subscriptioncreated;
        public LabelImageAddedService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;
            this.serviceBusNamespace = configuration.GetServiceBusNamespace();
            subscriptioncreated = CreateSubscription();
        }

        public async void ReceiveMessages()
        {
            var config = this.configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();

            if (!subscriptioncreated)
            {
                return;
            }

            var _queueClient = new SubscriptionClient(config.ConnectionString, TopicName, SubscriptionName);
            _queueClient.RegisterMessageHandler(MessageHandler, new MessageHandlerOptions(ExceptionHandler) { AutoComplete = false });
        }

        private bool CreateSubscription()
        {

            if (!serviceBusNamespace.Topics.List()
                .Any(t => t.Name
                .Equals(TopicName, StringComparison.InvariantCultureIgnoreCase)))
            {
                return false;
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

            return true;
        }

        private static Task MessageHandler(Message message, CancellationToken arg2)
        {
            Console.WriteLine($"message Label: {message.Label}");
            Console.WriteLine($"CorrelationId: {message.CorrelationId}");
            var prodChangesString = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("Message Received");
            Console.WriteLine(prodChangesString);

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
    }
}