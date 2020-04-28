using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Moogie.Queues.Tests
{
    public abstract class FakeProvider : IQueueProvider
    {
        public List<Deletable> DeletedMessages { get; set; } = new List<Deletable>();

        public List<Message> DispatchedMessages { get; set; } = new List<Message>();

        public List<Receivable> ReceivedMessages { get; set; } = new List<Receivable>();

        public virtual string ProviderName { get; } = "fake";

        public Task<DeleteResponse> Delete(Deletable deletable, CancellationToken cancellationToken = default)
        {
            DeletedMessages.Add(deletable);
            return Task.FromResult(new DeleteResponse());
        }

        public Task<DispatchResponse> Dispatch(Message message, CancellationToken cancellationToken = default)
        {
            DispatchedMessages.Add(message);
            return Task.FromResult(new DispatchResponse());
        }

        public Task<ReceiveResponse> Receive(Receivable receivable, CancellationToken cancellationToken = default)
        {
            ReceivedMessages.Add(receivable);
            return Task.FromResult(new ReceiveResponse());
        }
    }
}
