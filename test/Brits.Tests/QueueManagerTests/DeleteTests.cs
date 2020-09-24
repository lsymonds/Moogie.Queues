using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Brits.Tests
{
    public class DeleteTests : BaseQueueManagerTest
    {
        public static IEnumerable<object[]> DeleteParameters = new[]
        {
            new object[] {null!, typeof(ArgumentNullException)},
            new object[] {new TestDeletable { Queue = null}, typeof(MissingQueueException)}
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
            QueueManager.AddQueue("one", ProviderOne);
            QueueManager.AddQueue("two", ProviderTwo);

            var deletable = new TestDeletable { Queue = "two" };

            // Act.
            await QueueManager.Delete(deletable);

            // Assert.
            Assert.Empty(ProviderOne.DeletedMessages);
            Assert.Single(ProviderTwo.DeletedMessages);
        }
    }

    public class TestDeletable : Deletable
    {
    }
}
