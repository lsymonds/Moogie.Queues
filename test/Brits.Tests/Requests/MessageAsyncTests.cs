using System;
using System.Threading.Tasks;
using Xunit;

namespace Brits.Tests.Requests
{
    public class MessageAsyncTests
    {
        private readonly ExampleObjectToBeSerialised _content = new ExampleObjectToBeSerialised
        {
            Id = 1,
            Name = "foo"
        };
        
        [Fact]
        public async Task It_Adds_The_Id()
        {
            // Arrange.
            var guid = Guid.NewGuid();

            // Act.
            var message = await Message.WithContent(_content).WithId(guid);

            // Assert.
            Assert.Equal(guid, message.Id);
        }

        [Fact]
        public async Task It_Adds_The_Queue()
        {
            // Arrange.
            var queue = "default-queue";

            // Act.
            var message = await Message.WithContent(_content).OnQueue("default-queue");

            // Assert.
            Assert.Equal(queue, message.Queue);
        }

        [Fact]
        public async Task It_Sets_The_Content()
        {
            // Arrange & Act.
            var message = await Message.WithContent(_content);

            // Assert.
            Assert.Equal(
                "{\"Id\":1,\"Name\":\"foo\"}",
                message.Content
            );
        }

        [Fact]
        public async Task It_Adds_The_Expiry_Date()
        {
            // Arrange.
            var expiry = DateTime.Now.AddYears(1);

            // Act.
            var message = await Message.WithContent(_content).WhichExpiresAt(expiry);

            // Assert.
            Assert.Equal(expiry, message.Expiry);
        }

        [Fact]
        public async Task It_Defaults_The_Queue_If_Not_Specified()
        {
            // Arrange & Act.
            var message = await Message.WithContent(_content);

            // Assert.
            Assert.Equal("default", message.Queue);
        }
    }

    public class ExampleObjectToBeSerialised
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
