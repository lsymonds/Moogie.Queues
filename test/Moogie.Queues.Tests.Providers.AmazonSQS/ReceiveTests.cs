using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Queues.Tests.Providers.AmazonSQS
{
    public class ReceiveTests : BaseSQSProviderTests
    {
        [Fact]
        public async Task It_Receives_A_Message()
        {
            // Arrange.
            var id = Guid.NewGuid();
            await QueueManager.Dispatch(Message.OnQueue("default").WithContent("abc").WithId(id));

            // Act.
            var response = await QueueManager.Receive(1.Message().FromQueue("default"));

            // Assert.
            Assert.Single(response.Messages);
            Assert.Equal("abc", response.Messages.First().Content);
            Assert.Equal(id, response.Messages.First().Id);
            Assert.Equal("default", response.Messages.First().Queue);
            Assert.NotNull(response.Messages.First().Deletable);
        }

        [Fact]
        public async Task It_Does_Not_Receive_An_Expired_Message()
        {
            // Arrange.
            await QueueManager.Dispatch(Message.OnQueue("default")
                .WithContent("abc")
                .WhichExpiresAt(DateTime.Now.AddSeconds(1)));

            await Task.Delay(1000);

            // Act.
            var response = await QueueManager.Receive(1.Message().FromQueue("default"));

            // Assert.
            Assert.Empty(response.Messages);
        }
    }
}
