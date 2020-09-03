using System;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;

namespace Moogie.Queues.Tests.Providers.AmazonSQS
{
    public abstract class BaseSQSProviderTests
    {
        protected AmazonSQSClient SqsClient { get; }
        protected IQueueManager QueueManager { get; }
        protected string QueueUrl { get; }

        protected BaseSQSProviderTests()
        {
            var sqsOptions = new SQSProviderOptions
            {
                Credentials = new BasicAWSCredentials("abc", "def"),
                QueueUrl = "http://localhost:4566/000000000000/stuff",
                ClientConfig = new AmazonSQSConfig
                {
                    ServiceURL = "http://localhost:4566"
                }
            };

            SqsClient = new AmazonSQSClient(new BasicAWSCredentials("abc", "def"), sqsOptions.ClientConfig);
            var result = SqsClient.CreateQueueAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult();
            sqsOptions.QueueUrl = result.QueueUrl;
            QueueUrl = result.QueueUrl;

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
