using System.Threading.Tasks;
using Brits.Internal;
using Xunit;

namespace Brits.Tests.Responses
{
    public class ReceivedMessageTests
    {
        private class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        
        [Fact]
        public async Task It_Can_Deserialise_The_Content_Into_An_Object()
        {
            // Arrange.
            var obj = new TestObject { Id = 100, Name = "Foo" };
            var message = new ReceivedMessage { Content = await obj.Serialise() };
            
            // Act.
            var deserialised = await message.ReadContentAs<TestObject>();
            
            // Assert.
            Assert.Equal(100, deserialised.Id);
            Assert.Equal("Foo", deserialised.Name);
        }
    }
}
