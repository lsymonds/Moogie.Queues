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
            var dispatchable = new Dispatchable().WithId(guid);

            // Assert.
            Assert.Equal(guid, dispatchable.Id);
        }

        [Fact]
        public void It_Adds_The_Queue()
        {
            // Arrange.
            var queue = "default-queue";

            // Act.
            var dispatchable = new Dispatchable().OnQueue(queue);

            // Assert.
            Assert.Equal(queue, dispatchable.Queue);
        }

        [Fact]
        public void It_Adds_The_Text_Content()
        {
            // Arrange.
            var content = "Hello, worldies.";

            // Act.
            var dispatchable = new Dispatchable().WithContent(content);

            // Assert.
            Assert.Equal(content, dispatchable.Content);
        }

        [Fact]
        public void It_Adds_The_Expiry_Date()
        {
            // Arrange.
            var expiry = DateTime.Now.AddYears(1);

            // Act.
            var dispatchable = new Dispatchable().ExpireAt(expiry);

            // Assert.
            Assert.Equal(expiry, dispatchable.Expiry);
        }
    }
}
