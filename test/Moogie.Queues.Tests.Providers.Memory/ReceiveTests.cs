using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Queues.Tests.Providers.Memory
{
    public class ReceiveTests : BaseMemoryProviderTests
    {
        [Fact]
        public async Task It_Receives_Dispatched_Messages()
        {
            // Arrange.
            var messageOne = Message.OnQueue("default").WithContent("abc");
            var messageTwo = Message.OnQueue("default").WithContent("def");

            await QueueManager.Dispatch(messageOne);
            await QueueManager.Dispatch(messageTwo);

            // Act.
            var messages = await QueueManager.Receive(2.Messages().FromQueue("default"));

            // Assert.
            var listOfMessages = messages.Messages.ToList();

            Assert.Equal(2, listOfMessages.Count);
            Assert.Single(listOfMessages, x => x.Content == "abc");
            Assert.Single(listOfMessages, x => x.Content == "def");
        }

        [Fact]
        public async Task It_Does_Not_Receive_Expired_Messages()
        {
            // Arrange.
            await QueueManager.Dispatch(Message
                .OnQueue("default")
                .WithContent("abc")
                .WhichExpiresAt(DateTime.Now.AddSeconds(1)));

            await Task.Delay(1500);

            // Act.
            var messages = await QueueManager.Receive(1.Message().FromQueue("default"));

            // Assert.
            Assert.Equal(0, messages.Messages.Count());
        }

        [Fact]
        public async Task It_Long_Polls_For_Messages_Successfully()
        {
            // Arrange.
            var messageOne = Message.OnQueue("default").WithContent("abc");
            var messageTwo = Message.OnQueue("default").WithContent("def");

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
