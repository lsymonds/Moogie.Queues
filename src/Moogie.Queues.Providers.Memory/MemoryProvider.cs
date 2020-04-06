using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moogie.Queues.Providers.Memory
{
    /// <summary>
    /// In memory queue provider for testing purposes.
    /// </summary>
    public class MemoryProvider : IQueueProvider
    {
        private readonly ConcurrentDictionary<Guid, MessageRepresentation> _messages =
            new ConcurrentDictionary<Guid, MessageRepresentation>();

        /// <inheritdoc />
        public Task<DeleteResponse> Delete(Deletable deletable)
        {
            _messages.TryRemove(deletable.Id, out _);
            return Task.FromResult(new DeleteResponse());
        }

        /// <inheritdoc />
        public Task<DispatchResponse> Dispatch(Message message)
        {
            var messageId = message.Id ?? Guid.NewGuid();

            _messages.TryAdd(messageId, new MessageRepresentation
            {
                Id = messageId,
                Message = message.Content,
                Expiry = message.Expiry
            });

            return Task.FromResult(new DispatchResponse
            {
                MessageId = messageId
            });
        }

        /// <summary>
        /// Gets whether or not the memory provider contains the message.
        /// </summary>
        /// <param name="id">The id of the message.</param>
        public bool HasMessage(Guid id) => _messages.ContainsKey(id);

        /// <summary>
        /// Gets whether there are any messages in the memory provider.
        /// </summary>
        public bool HasMessages => _messages.Any();

        /// <inheritdoc />
        public async Task<ReceiveResponse> Receive(Receivable receivable)
            => receivable.SecondsToWait != null ? await LongPoll(receivable) : GetMessages(receivable);

        private async Task<ReceiveResponse> LongPoll(Receivable receivable)
        {
            var messagesReceived = new List<ReceiveResponse.ReceivedMessage>();
            var timeToWaitUntil = DateTime.Now.AddSeconds(receivable.SecondsToWait.Value);

            bool ShouldContinuePolling() => messagesReceived.Count < receivable.MessagesToReceive &&
                DateTime.Now <= timeToWaitUntil;

            while (ShouldContinuePolling())
            {
                messagesReceived.AddRange(GetMessages(receivable).Messages);
                if (ShouldContinuePolling())
                    await Task.Delay(250);
            }

            return new ReceiveResponse
            {
                Messages = messagesReceived
            };
        }

        private ReceiveResponse GetMessages(Receivable receivable)
        {
            var messages = _messages
                .Where(x => x.Value.Expiry == null || x.Value.Expiry > DateTime.Now)
                .Take(receivable.MessagesToReceive)
                .Select(x => new ReceiveResponse.ReceivedMessage
                {
                    Id = x.Value.Id,
                    Content = x.Value.Message
                });

            return new ReceiveResponse {Messages = messages};
        }
    }
}
