using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Queues.Tests
{
    public class DeleteTests : BaseQueueManagerTest
    {
        public static IEnumerable<object[]> DeleteParameters = new[]
        {
            new object[] {null, typeof(ArgumentNullException)},
            new object[] {new Deletable(), typeof(ArgumentException)},
            new object[] {new Deletable { ReceiptHandle = Guid.NewGuid().ToString()}, typeof(MissingQueueException)}
        };

        [Theory]
        [MemberData(nameof(DeleteParameters))]
        public async Task It_Validates_The_Delete_Parameter_Correctly(Deletable deletable, Type exceptionType)
        {
            // Arrange & Act.
            async Task Act() => await QueueManager.Delete(deletable);

            // Assert.
            await Assert.ThrowsAsync(exceptionType, Act);
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_Queue_Is_Not_Present()
        {
            // Arrange & Act.
            async Task Act() => await QueueManager.Delete(new Deletable
            {
                ReceiptHandle = "foo-bar",
                Queue = "wubwub"
            });

            // Assert.
            await Assert.ThrowsAsync<NoRegisteredQueueException>(Act);
        }

        [Fact]
        public async Task It_Attempts_To_Delete_The_Message_From_The_Queue()
        {
            // Arrange.
            var id = Guid.NewGuid();

            QueueManager.AddQueue("one", ProviderOne);
            QueueManager.AddQueue("two", ProviderTwo);

            var deletable = new Deletable
            {
                ReceiptHandle = "wubwub",
                Queue = "two"
            };

            // Act.
            await QueueManager.Delete(deletable);

            // Assert.
            Assert.Empty(ProviderOne.DeletedMessages);
            Assert.Single(ProviderTwo.DeletedMessages);
        }
    }
}
