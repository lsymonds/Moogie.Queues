using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moogie.Queues.Tests
{
    public abstract class FakeProvider : IQueueProvider
    {
        public List<IProviderDispatchable> DispatchedMessages { get; set; } = new List<IProviderDispatchable>();

        public List<IProviderReceivable> ReceivedMessages { get; set; } = new List<IProviderReceivable>();

        public Task<DispatchResponse> Dispatch(IProviderDispatchable dispatchable)
        {
            DispatchedMessages.Add(dispatchable);
            return Task.FromResult(new DispatchResponse());
        }

        public Task<ReceiveResponse> Receive(IProviderReceivable receivable)
        {
            ReceivedMessages.Add(receivable);
            return Task.FromResult(new ReceiveResponse());
        }
    }
}
