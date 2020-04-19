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
            new object[] {null!, typeof(ArgumentNullException)},
            new object[] {new Deletable { Queue = null}, typeof(MissingQueueException)},
            new object[] {new Deletable { Queue = "one"}, typeof(ArgumentNullException)},
            new object[] {new Deletable { DeletionAttributes = new Dictionary<string, string>() }, typeof(ArgumentNullException)}
        };
 
        [Theory]
        [MemberData(nameof(DeleteParameters))]
        public async Task It_Validates_The_Delete_Parameter_Correctly(Deletable deletable, Type exceptionType)
        {
            // Arrange.
            QueueManager.AddQueue("one", ProviderOne);

            // Act.
            async Task Act() => await QueueManager.Delete(deletable);

            // Assert.
            await Assert.ThrowsAsync(exceptionType, Act);
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
                Queue = "two",
                DeletionAttributes = new Dictionary<string, string> { { "MessageId", "Wub" } }
            };

            // Act.
            await QueueManager.Delete(deletable);

            // Assert.
            Assert.Empty(ProviderOne.DeletedMessages);
            Assert.Single(ProviderTwo.DeletedMessages);
        }
    }
}
