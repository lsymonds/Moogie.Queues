using System;
using System.Threading.Tasks;

namespace Moogie.Queues.Internal
{
    /// <summary>
    /// Base provider that implements common functionality shared across all providers.
    /// </summary>
    public abstract class BaseProvider : IQueueProvider
    {
        /// <inheritdoc />
        public abstract Task<DeleteResponse> Delete(Deletable deletable);

        /// <inheritdoc />
        public abstract Task<DispatchResponse> Dispatch(Message message);

        /// <inheritdoc />
        public abstract Task<ReceiveResponse> Receive(Receivable receivable);

        /// <summary>
        /// Deserialises a queue message into an instance of type <see cref="ReceivedMessage" />. If the received
        /// message has expired, then it is deleted from the queue and nothing is returned.
        /// </summary>
        /// <param name="content">The content to deserialise.</param>
        /// <param name="deletable">
        /// The <see cref="Deletable" /> instance to use should the message need to be deleted.
        /// </param>
        /// <returns>The deserialised <see cref="ReceivedMessage" />.</returns>
        protected async Task<ReceivedMessage> DeserialiseAndHandle(string content, Deletable deletable)
        {
            var deserialised = await content.TryDeserialise<ReceivedMessage>().ConfigureAwait(false);
            if (deserialised == null)
                return null;

            if (deserialised.Expiry != null && deserialised.Expiry < DateTime.Now)
            {
                await Delete(deletable).ConfigureAwait(false);
                return null;
            }

            deserialised.Deletable = deletable;
            return deserialised;
        }
    }
}