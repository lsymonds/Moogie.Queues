using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Queues.Tests.Providers.AzureQueueStorage
{
    public class DeleteTests : BaseAzureProviderTests
    {
        [Fact]
        public async Task It_Successfully_Deletes_A_Message()
        {
            // Arrange.
            await QueueManager.Dispatch(Message.OnQueue("default").WithContent("abc"));
            var receivedMessage = await QueueManager.Receive(1.Message().FromQueue("default"));

            // Act.
            var response = await QueueManager.Delete(receivedMessage.Messages.First().AsDeletable());

            // Assert.
            Assert.True(response.Success);
        }
    }
}