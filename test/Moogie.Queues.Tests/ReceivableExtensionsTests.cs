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
            var receivable = 1.Message().FromQueue(queue);

            // Assert.
            Assert.Equal(queue, receivable.Queue);
        }

        [Fact]
        public void It_Adds_The_Number_Of_Messages_To_Retrieve()
        {
            // Arrange & Act.
            var receivable = 10.Messages().FromQueue("abc");

            // Assert.
            Assert.Equal(10, receivable.MessagesToReceive);
        }
    }
}
