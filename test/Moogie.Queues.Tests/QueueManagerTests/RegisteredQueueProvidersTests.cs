using Xunit;

namespace Moogie.Queues.Tests
{
    public class RegisteredQueueProvidersTests : BaseQueueManagerTest
    {
        [Fact]
        public void It_Retrieves_The_Registered_Queue_Providers()
        {
            // Arrange.
            QueueManager.AddQueue("one", ProviderOne);
            QueueManager.AddQueue("two", ProviderTwo);
            
            // Act.
            var registeredProviders = QueueManager.RegisteredQueueProviders;

            // Assert.
            Assert.Equal(2, registeredProviders.Count);
            Assert.Single(
                registeredProviders, 
                provider => provider.Name == "one" && provider.ProviderName == "ProviderOne"
            );
            Assert.Single(
                registeredProviders, 
                provider => provider.Name == "two" && provider.ProviderName == "ProviderTwo"
            );
        }
    }
}