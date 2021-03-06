using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Brits.Tests.Providers.AzureQueueStorage
{
    public class ReceiveTests : BaseAzureProviderTests
    {
        [Fact]
        public async Task It_Receives_Messages_Correctly()
        {
            // Arrange.
            var messageOneId = Guid.NewGuid();
            var messageTwoId = Guid.NewGuid();
            var messageOne = Message.WithContent("abc").WithId(messageOneId);
            var messageTwo = Message.WithContent("def").WithId(messageTwoId);

            await QueueManager.Dispatch(messageOne);
            await QueueManager.Dispatch(messageTwo);

            // Act.
            var response = await QueueManager.Receive(2.Messages().FromQueue("default"));

            // Assert.
            Assert.NotNull(response);

            var listOfMessages = response.Messages.ToList();

            Assert.Equal(2, listOfMessages.Count);
            Assert.Contains(listOfMessages, message => message.Id == messageOneId);
            Assert.Contains(listOfMessages, message => message.Id == messageTwoId);
            Assert.Contains(listOfMessages, message => message.Content == "abc");
            Assert.Contains(listOfMessages, message => message.Content == "def");
        }

        [Fact]
        public async Task It_Does_Not_Receive_Expired_Messages()
        {
            // Arrange.
            await QueueManager.Dispatch(Message.WithContent("abc").WhichExpiresAt(DateTime.Now.AddSeconds(3)));

            await Task.Delay(4000);

            // Act.
            var response = await QueueManager.Receive(1.Message().FromQueue("default"));

            // Assert.
            Assert.Empty(response.Messages);
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_An_Attempt_Is_Made_To_Long_Poll()
        {
            // Arrange & Act.
            async Task Act() => await QueueManager.Receive(2.Messages().FromQueue("default").ButWaitFor(5));

            // Assert.
            await Assert.ThrowsAsync<FeatureNotYetSupportedException>(Act);
        }

        [Fact]
        public async Task It_Does_Not_Thrown_An_Exception_When_Ignore_Long_Poll_Is_True()
        {
            // Arrange.
            SetDependencies(false);

            // Act.
            var messages = await QueueManager.Receive(1.Message().FromQueue("default").ButWaitFor(5));

            // Assert.
            Assert.Empty(messages.Messages);
        }
    }
}
