using System;
using System.Collections.Generic;
using Xunit;

namespace Moogie.Queues.Tests
{
    public class DeletableTests
    {
        [Fact]
        public void It_Converts_Successfully_From_Message()
        {
            // Arrange.
            var message = new Message
            {
                Id = Guid.NewGuid()
            };

            // Act.
            var deletable = message.AsDeletable();

            // Assert.
            Assert.Equal(message.Id, deletable.Id);
        }

        [Fact]
        public void It_Converts_Successfully_From_Id()
        {
            // Arrange.
            var id = Guid.NewGuid();

            // Act.
            var deletable = Deletable.FromId(id);

            // Assert.
            Assert.Equal(id, deletable.Id);
        }
    }
}
