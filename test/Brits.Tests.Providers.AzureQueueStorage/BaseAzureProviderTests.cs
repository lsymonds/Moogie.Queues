using System;
using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;

namespace Brits.Tests.Providers.AzureQueueStorage
{
    public abstract class BaseAzureProviderTests
    {
        protected QueueClient QueueClient = null!;
        protected IQueueManager QueueManager = null!;

        public BaseAzureProviderTests() => SetDependencies();

        protected void SetDependencies(bool throwOnLongPoll = true)
        {
            var options = new AzureQueueStorageOptions
            {
                ConnectionString = "UseDevelopmentStorage=true",
                QueueName = Guid.NewGuid().ToString(),
                IgnoreLongPollingException = !throwOnLongPoll
            };

            QueueClient = new QueueClient(options.ConnectionString, options.QueueName);
            QueueClient.CreateIfNotExists();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddBrits(new QueueRegistration
            {
                Name = "default",
                QueueProvider = new AzureQueueStorageProvider(options)
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            QueueManager = serviceProvider.GetService<IQueueManager>();
        }
    }
}
