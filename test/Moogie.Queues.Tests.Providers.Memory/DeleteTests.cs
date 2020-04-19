using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Queues.Tests.Providers.Memory
{
    public class DeleteTests : BaseMemoryProviderTests
    {
        [Fact]
        public async Task It_Continues_As_Normal_When_Message_Does_Not_Exist()
        {
            // Arrange.
            var id = Guid.NewGuid();

            // Act.
            await QueueManager.Delete(new Deletable
            {
                Queue = "default",
                DeletionAttributes = new Dictionary<string, string>
                {
                    { "MessageId", id.ToString() }
                }
            });

            // Assert.
            Assert.False(QueueProvider.HasMessage(id));
        }

        [Fact]
        public async Task It_Removes_A_Dispatched_Message_From_The_Queue()
        {
            // Arrange.
            var id = Guid.NewGuid();

            var message = Message.WithContent("abc").WithId(id);
            await QueueManager.Dispatch(message);

            // Act.
            var receivedMessages = await QueueManager.Receive(1.Message().FromQueue("default"));
            await QueueManager.Delete(receivedMessages.Messages.First().Deletable);

            // Assert.
            Assert.False(QueueProvider.HasMessage(id));
        }
    }
}
