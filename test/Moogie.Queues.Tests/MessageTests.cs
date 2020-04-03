using System;
using Xunit;

namespace Moogie.Queues.Tests
{
    public class DispatchableExtensionsTests
    {
        [Fact]
        public void It_Adds_The_Id()
        {
            // Arrange.
            var guid = Guid.NewGuid();

            // Act.
            var message = Message.OnQueue("abc").WithId(guid);

            // Assert.
            Assert.Equal(guid, message.Id);
        }

        [Fact]
        public void It_Adds_The_Queue()
        {
            // Arrange.
            var queue = "default-queue";

            // Act.
            var message = Message.OnQueue("default-queue");

            // Assert.
            Assert.Equal(queue, message.Queue);
        }

        [Fact]
        public void It_Adds_The_Text_Content()
        {
            // Arrange.
            var content = "Hello, worldies.";

            // Act.
            var message = Message.OnQueue("abc").WithContent(content);

            // Assert.
            Assert.Equal(content, message.Content);
        }

        [Fact]
        public void It_Adds_The_Expiry_Date()
        {
            // Arrange.
            var expiry = DateTime.Now.AddYears(1);

            // Act.
            var message = Message.OnQueue("abc").WhichExpiresAt(expiry);

            // Assert.
            Assert.Equal(expiry, message.Expiry);
        }
    }
}
