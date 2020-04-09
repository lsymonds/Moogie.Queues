using System;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using Moogie.Queues.Providers.AmazonSQS;

namespace Moogie.Queues.Tests.Providers.AmazonSQS
{
    public abstract class BaseSQSProviderTests
    {
        protected AmazonSQSClient SqsClient { get; }
        protected IQueueManager QueueManager { get; }

        protected BaseSQSProviderTests()
        {
            SqsClient = new AmazonSQSClient();

            var queueUrl = Environment.GetEnvironmentVariable("SQS_QUEUE_URL");

            var sqsOptions = new SQSOptions
            {
                Credentials = new BasicAWSCredentials("abc", "def"),
                QueueUrl = queueUrl,
                ClientConfig = new AmazonSQSConfig()
            };

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMoogieQueues(new QueueRegistration
            {
                Name = "default",
                QueueProvider = new SQSProvider(sqsOptions)
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            QueueManager = serviceProvider.GetService<IQueueManager>();
        }
    }
}
