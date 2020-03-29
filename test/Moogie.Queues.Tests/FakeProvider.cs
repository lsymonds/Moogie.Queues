using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moogie.Queues.Tests
{
    public abstract class FakeProvider : IQueueProvider
    {
        public List<IProviderDispatchable> DispatchedMessages { get; set; } = new List<IProviderDispatchable>();

        public List<IProviderReceivable> ReceivedMessages { get; set; } = new List<IProviderReceivable>();

        public async Task<DispatchResponse> Dispatch(IProviderDispatchable dispatchable)
        {
            DispatchedMessages.Add(dispatchable);

            return new DispatchResponse();
        }

        public async Task<ReceiveResponse> Receive(IProviderReceivable receivable)
        {
            ReceivedMessages.Add(receivable);

            return new ReceiveResponse();
        }
    }
}
