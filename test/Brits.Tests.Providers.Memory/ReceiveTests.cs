using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Brits.Tests.Providers.Memory
{
    public class ReceiveTests : BaseMemoryProviderTests
    {
        [Fact]
        public async Task It_Receives_Dispatched_Messages()
        {
            // Arrange.
            var messageOne = Message.WithContent("abc");
            var messageTwo = Message.WithContent("def");

            await QueueManager.Dispatch(messageOne);
            await QueueManager.Dispatch(messageTwo);

            // Act.
            var messages = await QueueManager.Receive(2.Messages().FromQueue("default"));

            // Assert.
            var listOfMessages = messages.Messages.ToList();

            Assert.Equal(2, listOfMessages.Count);
            Assert.Single(listOfMessages, message => message.Content == "abc");
            Assert.Single(listOfMessages, x => x.Content == "def");
        }

        [Fact]
        public async Task It_Does_Not_Receive_Expired_Messages()
        {
            // Arrange.
            await QueueManager.Dispatch(Message.WithContent("abc").WhichExpiresAt(DateTime.Now.AddSeconds(1)));

            await Task.Delay(1500);

            // Act.
            var messages = await QueueManager.Receive(1.Message().FromQueue("default"));

            // Assert.
            Assert.Empty(messages.Messages);
        }

        [Fact]
        public async Task It_Long_Polls_For_Messages_Successfully()
        {
            // Arrange.
            var messageOne = Message.WithContent("abc");
            var messageTwo = Message.WithContent("def");

            await QueueManager.Dispatch(messageOne);

            // Dispatch the message a second later in an alternate thread.
#pragma warning disable 4014
            Task.Run(async () =>
#pragma warning restore 4014
            {
                await Task.Delay(1000);
                await QueueManager.Dispatch(messageTwo);
            });

            // Act.
            var messages = await QueueManager.Receive(2.Messages().FromQueue("default").ButWaitFor(5));

            // Assert.
            Assert.Equal(2, messages.Messages.Count());
        }
    }
}
