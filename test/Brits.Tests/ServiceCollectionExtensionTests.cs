using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Brits.Tests
{
    public class ServiceCollectionExtensionTests
    {
        [Fact]
        public async Task It_Adds_The_Queue_Manager_To_The_Service_Collection()
        {
            // Arrange.
            var serviceCollection = new ServiceCollection();

            var providerOne = new ProviderOne();
            var providerTwo = new ProviderTwo();
            serviceCollection.AddBrits(
                new QueueRegistration { Name = "one", QueueProvider = providerOne },
                new QueueRegistration { Name = "two", QueueProvider = providerTwo }
            );

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var queueManager = serviceProvider.GetService<IQueueManager>();

            // Act.
            await queueManager.Dispatch(Message.WithContent("hello, world").OnQueue("two"));

            // Assert.
            Assert.Empty(providerOne.DispatchedMessages);
            Assert.Single(providerTwo.DispatchedMessages, message => message.Content == "hello, world");
        }

        private class ProviderOne : FakeProvider
        {
        }

        private class ProviderTwo : FakeProvider
        {
        }
    }
}
