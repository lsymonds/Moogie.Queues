using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moogie.Queues.Internal
{
    /// <summary>
    /// Base provider that implements common functionality shared across all providers.
    /// </summary>
    public abstract class BaseProvider<TDeletable> : IQueueProvider where TDeletable : Deletable
    {
        /// <inheritdoc />
        public abstract string ProviderName { get; }

        /// <inheritdoc />
        public abstract Task<DeleteResponse> Delete(
            Deletable deletable,
            CancellationToken cancellationToken = default
        );

        /// <inheritdoc />
        public abstract Task<DispatchResponse> Dispatch(Message message, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public abstract Task<ReceiveResponse> Receive(
            Receivable receivable,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deserialises a queue message into an instance of type <see cref="ReceivedMessage" />. If the received
        /// message has expired, then it is deleted from the queue and nothing is returned.
        /// </summary>
        /// <param name="content">The content to deserialise.</param>
        /// <param name="deletable">
        /// The <see cref="Deletable" /> instance to use should the message need to be deleted.
        /// </param>
        /// <param name="cancellationToken">The cancellation token to use in asynchronous operations.</param>
        /// <returns>The deserialised <see cref="ReceivedMessage" />.</returns>
        protected async Task<ReceivedMessage> DeserialiseAndHandle(
            string content, 
            TDeletable deletable,
            CancellationToken cancellationToken
        )
        {
            var deserialised = await content
                .TryDeserialise<ReceivedMessage>(cancellationToken)
                .ConfigureAwait(false);
            
            if (deserialised == null)
                return null;

            if (deserialised.Expiry != null && deserialised.Expiry < DateTime.Now)
            {
                await Delete(deletable, cancellationToken).ConfigureAwait(false);
                return null;
            }

            deserialised.Deletable = deletable;
            return deserialised;
        }

        protected TDeletable CastAndValidate(
            Deletable deletable, 
            params Func<TDeletable, (bool success, string attributeName)>[] validationCriteria
        )
        {
            var casted = deletable as TDeletable;
            foreach (var criteria in validationCriteria)
            {
                var criteriaResponse = criteria(casted);
                if (!criteriaResponse.success)
                    throw new ArgumentNullException();
            }

            return casted;
        }
        
    }
}