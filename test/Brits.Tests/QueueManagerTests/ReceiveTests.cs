using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Brits.Tests
{
    public class ReceiveTests : BaseQueueManagerTest
    {
        public static IEnumerable<object[]> ValidationEntities = new[]
        {
            new object[] { null!, typeof(ArgumentNullException) },

            new object[] { new Receivable { Queue = null }, typeof(MissingQueueException) },
            new object[]
            {
                new Receivable { Queue = "foo", MessagesToReceive = -3 },
                typeof(InvalidMessagesToReceiveParameterException)
            }
        };

        [Theory]
        [MemberData(nameof(ValidationEntities))]
        public async Task It_Validates_Receivable_Entity_Correctly(Receivable receivable, Type exceptionType)
        {
            // Arrange & Act.
            async Task Act() => await QueueManager.Receive(receivable);

            // Assert.
            await Assert.ThrowsAsync(exceptionType, Act);
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_An_Attempt_Is_Made_To_Add_A_Message_To_A_Non_Existent_Queue()
        {
            // Arrange & Act.
            async Task Act() => await QueueManager.Receive(1.Message().FromQueue("random"));

            // Assert.
            await Assert.ThrowsAsync<NoRegisteredQueueException>(Act);
        }

        [Fact]
        public async Task It_Receives_Messages_From_The_Appropriate_Queue()
        {
            // Arrange.
            QueueManager.AddQueue("one", ProviderOne);
            QueueManager.AddQueue("two", ProviderTwo);

            // Act.
            await QueueManager.Receive(10.Messages().FromQueue("two"));

            // Assert.
            Assert.Empty(ProviderOne.ReceivedMessages);
            Assert.Single(ProviderTwo.ReceivedMessages);
        }
    }
}
