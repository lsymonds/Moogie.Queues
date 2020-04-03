using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Moogie.Queues.Validators;

namespace Moogie.Queues
{
    /// <summary>
    /// Manages all aspects of the queueing systems.
    /// </summary>
    public class QueueManager : IQueueManager
    {
        private readonly ConcurrentDictionary<string, IQueueProvider> _queueProviders
            = new ConcurrentDictionary<string, IQueueProvider>();

        /// <inheritdoc />
        public void AddQueue(string queue, IQueueProvider queueProvider)
        {
            if (string.IsNullOrWhiteSpace(queue)) throw new ArgumentNullException(nameof(queue));
            if (_queueProviders.ContainsKey(queue))
                throw new DuplicateQueueException(queue);

            _queueProviders[queue] = queueProvider ?? throw new ArgumentNullException(nameof(queueProvider));
        }

        /// <inheritdoc />
        public async Task<DeleteResponse> Delete(Deletable deletable)
        {
            DeletableValidator.Validate(deletable);
            return await GetProvider(deletable.Queue).Delete(deletable).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<DispatchResponse> Dispatch(Message message)
        {
            MessageValidator.Validate(message);
            return await GetProvider(message.Queue).Dispatch(message).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<ReceiveResponse> Receive(Receivable receivable)
        {
            ReceivableValidator.Validate(receivable);
            return await GetProvider(receivable.Queue).Receive(receivable).ConfigureAwait(false);
        }

        private IQueueProvider GetProvider(string queue)
        {
            if (!_queueProviders.ContainsKey(queue))
                throw new NoRegisteredQueueException(queue);

            return _queueProviders[queue];
        }
    }
}
