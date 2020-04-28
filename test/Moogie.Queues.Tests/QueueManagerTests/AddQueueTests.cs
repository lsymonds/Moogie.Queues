using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Queues.Tests
{
    public class AddQueueTests
    {
        [Fact]
        public void It_Throws_An_Exception_When_Attempting_To_Add_A_Queue_Without_A_Name()
        {
            // Arrange.
            string? name = null;

            // Act.
            void Act() => new QueueManager().AddQueue(name!, null!);

            // Assert.
            Assert.Throws<ArgumentNullException>(Act);
        }

        [Fact]
        public void It_Throws_An_Exception_When_Attempting_To_Add_A_Queue_With_An_Invalid_Provider()
        {
            // Arrange.
            IQueueProvider provider = null!;

            // Act.
            void Act() => new QueueManager().AddQueue("default", provider);

            // Assert.
            Assert.Throws<ArgumentNullException>(Act);
        }

        [Fact]
        public void It_Can_Add_Provider_To_The_Internal_Collection_Without_An_Exception_Being_Thrown()
        {
            // Arrange.
            IQueueProvider provider = new FakeProvider();

            // Act.
            new QueueManager().AddQueue("default", provider);
        }

        [Fact]
        public void It_Throws_An_Exception_When_Attempting_To_Add_A_Queue_With_A_Name_That_Is_Already_Registered()
        {
            // Arrange.
            var manager = new QueueManager();
            manager.AddQueue("default", new FakeProvider());

            // Act.
            void Act() => manager.AddQueue("default", new FakeProvider());

            // Assert.
            Assert.Throws<DuplicateQueueException>(Act);
        }

        private class FakeProvider : IQueueProvider
        {
            public string ProviderName { get; } = "fake";

            public Task<DeleteResponse> Delete(Deletable deletable, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<DispatchResponse> Dispatch(Message message, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<ReceiveResponse> Receive(Receivable receivable, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }
    }
}
