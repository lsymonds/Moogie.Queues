using System;
using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;

namespace Moogie.Queues.Tests.Providers.AzureQueueStorage
{
    public abstract class BaseAzureProviderTests
    {
        protected readonly QueueClient QueueClient;
        protected readonly IQueueManager QueueManager;

        public BaseAzureProviderTests()
        {
            var options = new AzureQueueStorageOptions
            {
                ConnectionString = "UseDevelopmentStorage=true",
                QueueName = Guid.NewGuid().ToString()
            };

            QueueClient = new QueueClient(options.ConnectionString, options.QueueName);
            QueueClient.CreateIfNotExists();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMoogieQueues(new QueueRegistration
            {
                Name = "default",
                QueueProvider = new AzureQueueStorageProvider(options)
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            QueueManager = serviceProvider.GetService<IQueueManager>();
        }
    }
}