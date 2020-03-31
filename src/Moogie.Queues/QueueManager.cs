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

        /// <summary>
        /// Adds a queue provider to the collection of named queue providers maintained in the current
        /// <see cref="QueueManager"/> instance.
        /// </summary>
        /// <param name="queue">The name of the queue provider.</param>
        /// <param name="queueProvider">The <see cref="IQueueProvider"/> implementation.</param>
        public void AddQueue(string queue, IQueueProvider queueProvider)
        {
            if (string.IsNullOrWhiteSpace(queue)) throw new ArgumentNullException(nameof(queue));
            if (queueProvider == null) throw new ArgumentNullException(nameof(queueProvider));

            if (_queueProviders.ContainsKey(queue))
                throw new DuplicateQueueException(queue);

            _queueProviders[queue] = queueProvider;
        }

        /// <summary>
        /// Dispatches a message onto a specified queue.
        /// </summary>
        /// <param name="dispatchable">
        /// The message configuration object which contains all of the necessary properties.
        /// </param>
        /// <returns>
        /// An awaitable task yielding the response from the attempt to dispatch the message onto the queue.
        /// </returns>
        public async Task<DispatchResponse> Dispatch(Dispatchable dispatchable)
        {
            DispatchableValidator.Validate(dispatchable);
            return await GetProvider(dispatchable.Queue).Dispatch(dispatchable).ConfigureAwait(false);
        }

        /// <summary>
        /// Listens to and receives a message from a specified queue.
        /// </summary>
        /// <param name="receivable">
        /// The configuration object which determines how messages will be read from the specified queue.
        /// </param>
        /// <returns>
        /// An awaitable task yielding the response from the attempt to receive the message(s) from the queue.
        /// </returns>
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
