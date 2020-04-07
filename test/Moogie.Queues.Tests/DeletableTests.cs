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
            var message = new ReceivedMessage
            {
                ReceiptHandle = "foo-bar"
            };

            // Act.
            var deletable = message.AsDeletable();

            // Assert.
            Assert.Equal(message.ReceiptHandle, deletable.ReceiptHandle);
        }
    }
}
