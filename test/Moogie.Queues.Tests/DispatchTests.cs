using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Queues.Tests
{
    public class DispatchTests
    {
        private readonly IQueueManager _queueManager = new QueueManager();

        public static IEnumerable<object[]> ValidationEntities = new[]
        {
            new object[] { null!, typeof(ArgumentNullException) },
            new object[] { Dispatchable.OnQueue("foo"), typeof(MissingContentException) },
            new object[]
            {
                Dispatchable.OnQueue("foo").WithContent("abc").ExpireAt(DateTime.Now.AddDays(-1)),
                typeof(InvalidDispatchableExpiryException)
            }
        };

        [Theory]
        [MemberData(nameof(ValidationEntities))]
        public async Task It_Validates_Dispatchable_Entity_Correctly(Dispatchable dispatchable, Type exceptionType)
        {
            // Arrange & Act.
            async Task Act() => await _queueManager.Dispatch(dispatchable);

            // Assert.
            await Assert.ThrowsAsync(exceptionType, Act);
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_An_Attempt_Is_Made_To_Add_A_Message_To_A_Non_Existent_Queue()
        {
            // Arrange & Act.
            async Task Act() => await _queueManager.Dispatch(Dispatchable.OnQueue("random").WithContent("foo"));

            // Assert.
            await Assert.ThrowsAsync<NoRegisteredQueueException>(Act);
        }

        [Fact]
        public async Task It_Dispatches_Messages_To_The_Appropriate_Queue()
        {
            // Arrange.
            var providerOne = new ProviderOne();
            var providerTwo = new ProviderTwo();

            _queueManager.AddQueue("provider-one", providerOne);
            _queueManager.AddQueue("provider-two", providerTwo);

            var id = Guid.NewGuid();
            var secondId = Guid.NewGuid();

            var expiry = DateTime.Now.AddMonths(1);

            // Act.
            await _queueManager.Dispatch(Dispatchable
                .OnQueue("provider-one")
                .WithId(id)
                .WithContent("hello, provider one"));

            await _queueManager.Dispatch(Dispatchable
                .OnQueue("provider-two")
                .WithId(secondId)
                .WithContent("hello, provider two")
                .ExpireAt(expiry));

            // Assert.
            Assert.Single(providerOne.DispatchedMessages);
            Assert.Single(providerTwo.DispatchedMessages);

            Assert.Equal(id, providerOne.DispatchedMessages.First().Id);
            Assert.Equal(secondId, providerTwo.DispatchedMessages.First().Id);

            Assert.Equal("hello, provider one", providerOne.DispatchedMessages.First().Content);
            Assert.Equal("hello, provider two", providerTwo.DispatchedMessages.First().Content);

            Assert.Equal(expiry, providerTwo.DispatchedMessages.First().Expiry);
        }

        public class ProviderOne : FakeProvider
        {
        }

        public class ProviderTwo : FakeProvider
        {
        }
    }
}
