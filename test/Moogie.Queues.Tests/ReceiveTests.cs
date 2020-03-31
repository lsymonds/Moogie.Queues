using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Queues.Tests
{
    public class ReceiveTests
    {
        private readonly IQueueManager _queueManager = new QueueManager();

        public static IEnumerable<object[]> ValidationEntities = new[]
        {
            new object[] { null!, typeof(ArgumentNullException) },
            new object[]
            {
                Receivable.FromQueue("foo").AmountOfMessagesToReceive(0),
                typeof(InvalidMessagesToReceiveParameter)
            }
        };

        [Theory]
        [MemberData(nameof(ValidationEntities))]
        public async Task It_Validates_Receivable_Entity_Correctly(Receivable receivable, Type exceptionType)
        {
            // Arrange & Act.
            async Task Act() => await _queueManager.Receive(receivable);

            // Assert.
            await Assert.ThrowsAsync(exceptionType, Act);
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_An_Attempt_Is_Made_To_Add_A_Message_To_A_Non_Existent_Queue()
        {
            // Arrange & Act.
            async Task Act() => await _queueManager.Receive(Receivable.FromQueue("random"));

            // Assert.
            await Assert.ThrowsAsync<NoRegisteredQueueException>(Act);
        }

        [Fact]
        public async Task It_Receives_Messages_From_The_Appropriate_Queue()
        {
            // Arrange.
            var providerOne = new ProviderOne();
            var providerTwo = new ProviderTwo();

            _queueManager.AddQueue("one", providerOne);
            _queueManager.AddQueue("two", providerTwo);

            // Act.
            await _queueManager.Receive(Receivable.FromQueue("two").AmountOfMessagesToReceive(10));

            // Assert.
            Assert.Empty(providerOne.ReceivedMessages);
            Assert.Single(providerTwo.ReceivedMessages);
        }

        private class ProviderOne : FakeProvider
        {
        }

        private class ProviderTwo : FakeProvider
        {
        }
    }
}
