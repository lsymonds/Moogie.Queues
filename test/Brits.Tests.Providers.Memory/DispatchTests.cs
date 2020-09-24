using System;
using System.Threading.Tasks;
using Xunit;

namespace Brits.Tests.Providers.Memory
{
    public class DispatchTests : BaseMemoryProviderTests
    {
        [Fact]
        public async Task It_Dispatches_A_Message()
        {
            // Arrange.
            var id = Guid.NewGuid();
            var message = Message.WithContent("Hello, worldington.").WithId(id);

            // Act.
            var response = await QueueManager.Dispatch(message);

            // Assert.
            Assert.Equal(id, response.MessageId);
            Assert.True(QueueProvider.HasMessage(id));
        }

        [Fact]
        public async Task It_Assigns_An_Id_To_A_Dispatched_Message_Without_One()
        {
            // Arrange.
            var message = Message.WithContent("hi");

            // Act.
            var response = await QueueManager.Dispatch(message);

            // Assert.
            Assert.NotEqual(Guid.Empty, response.MessageId);
            Assert.True(QueueProvider.HasMessages);
        }
    }
}
