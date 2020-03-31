using Xunit;

namespace Moogie.Queues.Tests
{
    public class ReceivableExtensionsTests
    {
        [Fact]
        public void It_Adds_The_Queue()
        {
            // Arrange.
            var queue = "receivable-queue";

            // Act.
            var receivable = Receivable.FromQueue(queue);

            // Assert.
            Assert.Equal(queue, receivable.Queue);
        }

        [Fact]
        public void It_Adds_The_Number_Of_Messages_To_Retrieve()
        {
            // Arrange.
            var messagesToReceive = 10u;

            // Act.
            var receivable = Receivable.FromQueue("abc").AmountOfMessagesToReceive(messagesToReceive);

            // Assert.
            Assert.Equal(messagesToReceive, receivable.MessagesToReceive);
        }
    }
}
