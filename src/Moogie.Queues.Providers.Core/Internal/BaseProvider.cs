using System;
using System.Threading.Tasks;

namespace Moogie.Queues.Internal
{
    public abstract class BaseProvider : IQueueProvider
    {
        /// <inheritdoc />
        public abstract Task<DeleteResponse> Delete(Deletable deletable);

        /// <inheritdoc />
        public abstract Task<DispatchResponse> Dispatch(Message message);

        /// <inheritdoc />
        public abstract Task<ReceiveResponse> Receive(Receivable receivable);

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