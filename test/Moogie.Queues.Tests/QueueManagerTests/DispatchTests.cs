using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Queues.Tests
{
    public class DispatchTests : BaseQueueManagerTest
    {
        public static IEnumerable<object[]> ValidationEntities = new[]
        {
            new object[] { null!, typeof(ArgumentNullException) },
            new object[] { new Message(), typeof(MissingQueueException) },
            new object[] { Message.OnQueue("foo"), typeof(MissingContentException) },
            new object[]
            {
                Message.OnQueue("foo").WithContent("abc").WhichExpiresAt(DateTime.Now.AddDays(-1)),
                typeof(InvalidDispatchableExpiryException)
            }
        };

        [Theory]
        [MemberData(nameof(ValidationEntities))]
        public async Task It_Validates_Message_Entity_Correctly(Message message, Type exceptionType)
        {
            // Arrange & Act.
            async Task Act() => await QueueManager.Dispatch(message);

            // Assert.
            await Assert.ThrowsAsync(exceptionType, Act);
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_An_Attempt_Is_Made_To_Add_A_Message_To_A_Non_Existent_Queue()
        {
            // Arrange & Act.
            async Task Act() => await QueueManager.Dispatch(Message.OnQueue("random").WithContent("foo"));

            // Assert.
            await Assert.ThrowsAsync<NoRegisteredQueueException>(Act);
        }

        [Fact]
        public async Task It_Dispatches_Messages_To_The_Appropriate_Queue()
        {
            // Arrange.
            QueueManager.AddQueue("provider-one", ProviderOne);
            QueueManager.AddQueue("provider-two", ProviderTwo);

            var id = Guid.NewGuid();
            var secondId = Guid.NewGuid();

            var expiry = DateTime.Now.AddMonths(1);

            // Act.
            await QueueManager.Dispatch(Message
                .OnQueue("provider-one")
                .WithId(id)
                .WithContent("hello, provider one"));

            await QueueManager.Dispatch(Message
                .OnQueue("provider-two")
                .WithId(secondId)
                .WithContent("hello, provider two")
                .WhichExpiresAt(expiry));

            // Assert.
            Assert.Single(ProviderOne.DispatchedMessages);
            Assert.Single(ProviderTwo.DispatchedMessages);

            Assert.Equal(id, ProviderOne.DispatchedMessages.First().Id);
            Assert.Equal(secondId, ProviderTwo.DispatchedMessages.First().Id);

            Assert.Equal("hello, provider one", ProviderOne.DispatchedMessages.First().Content);
            Assert.Equal("hello, provider two", ProviderTwo.DispatchedMessages.First().Content);

            Assert.Equal(expiry, ProviderTwo.DispatchedMessages.First().Expiry);
        }
    }
}
