using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moogie.Queues.Tests
{
    public abstract class FakeProvider : IQueueProvider
    {
        public List<Message> DispatchedMessages { get; set; } = new List<Message>();

        public List<Receivable> ReceivedMessages { get; set; } = new List<Receivable>();

        public Task<DispatchResponse> Dispatch(Message message)
        {
            DispatchedMessages.Add(message);
            return Task.FromResult(new DispatchResponse());
        }

        public Task<ReceiveResponse> Receive(Receivable receivable)
        {
            ReceivedMessages.Add(receivable);
            return Task.FromResult(new ReceiveResponse());
        }
    }
}
