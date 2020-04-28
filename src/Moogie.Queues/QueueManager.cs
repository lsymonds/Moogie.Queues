using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public IReadOnlyList<RegisteredQueueProvider> RegisteredQueueProviders => 
            _queueProviders.Select(
                x => new RegisteredQueueProvider {Name = x.Key, ProviderName = x.Value.ProviderName}
            ).ToList();

        /// <inheritdoc />
        public void AddQueue(string queue, IQueueProvider queueProvider)
        {
            if (string.IsNullOrWhiteSpace(queue)) throw new ArgumentNullException(nameof(queue));
            if (_queueProviders.ContainsKey(queue))
                throw new DuplicateQueueException(queue);

            _queueProviders[queue] = queueProvider ?? throw new ArgumentNullException(nameof(queueProvider));
        }

        /// <inheritdoc />
        public Task<DeleteResponse> Delete(ReceivedMessage message, CancellationToken cancellationToken = default) 
            => Delete(message.Deletable, cancellationToken);

        /// <inheritdoc />
        public async Task<DeleteResponse> Delete(Deletable deletable, CancellationToken cancellationToken = default)
        {
            DeletableValidator.Validate(deletable);
            return await GetProvider(deletable.Queue).Delete(deletable, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<DispatchResponse> Dispatch(Message message, CancellationToken cancellationToken = default)
        {
            MessageValidator.Validate(message);
            message.Id = message.Id == Guid.Empty ? Guid.NewGuid() : message.Id;

            return await GetProvider(message.Queue).Dispatch(message, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<ReceiveResponse> Receive(Receivable receivable, CancellationToken cancellationToken = default)
        {
            ReceivableValidator.Validate(receivable);
            return await GetProvider(receivable.Queue).Receive(receivable, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Takes a queue name and returns a queue provider registered by that name. If there is no queue registered
        /// by that name, then a <see cref="NoRegisteredQueueException" /> exception is thrown.
        /// </summary>
        /// <param name="queue">The queue name to search by.</param>
        /// <returns>An instance of a <see cref="IQueueProvider" /> implementation.</returns>
        private IQueueProvider GetProvider(string queue)
        {
            if (!_queueProviders.ContainsKey(queue))
                throw new NoRegisteredQueueException(queue);

            return _queueProviders[queue];
        }
    }
}
