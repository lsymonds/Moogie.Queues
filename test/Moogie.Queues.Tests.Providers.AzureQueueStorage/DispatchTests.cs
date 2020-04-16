using System;
using System.Threading.Tasks;
using Moogie.Queues.Internal;
using Xunit;

namespace Moogie.Queues.Tests.Providers.AzureQueueStorage
{
    public class DispatchTests : BaseAzureProviderTests
    {
        [Fact]
        public async Task It_Dispatches_A_Message()
        {
            // Arrange.
            var messageId = Guid.NewGuid();
            var expiry = DateTime.Now.AddDays(5);
            var message = Message.OnQueue("default")
                .WithId(messageId)
                .WithContent("abc")
                .WhichExpiresAt(expiry);

            // Act.
            var response = await QueueManager.Dispatch(message);

            // Assert.
            Assert.NotNull(response);
            Assert.Equal(messageId, message.Id);

            var receivedMessages = await QueueClient.ReceiveMessagesAsync(1);
            var deserialisedMessage = await receivedMessages.Value[0].MessageText.TryDeserialise<ReceivedMessage>();

            Assert.NotNull(deserialisedMessage);
            Assert.Equal(messageId, deserialisedMessage.Id);
            Assert.Equal("default", deserialisedMessage.Queue);
            Assert.Equal("abc", deserialisedMessage.Content);
            Assert.Equal(expiry, deserialisedMessage.Expiry);
        }
    }
}