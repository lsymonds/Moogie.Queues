using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moogie.Queues.Internal;

namespace Moogie.Queues
{
    /// <summary>
    /// In memory queue provider for testing purposes.
    /// </summary>
    public class MemoryProvider : BaseProvider<MemoryDeletable>
    {
        private readonly ConcurrentDictionary<string, QueueableMessage> _messages =
            new ConcurrentDictionary<string, QueueableMessage>();

        /// <inheritdoc />
        public override string ProviderName { get; } = nameof(MemoryProvider);

        /// <inheritdoc />
        public override Task<DeleteResponse> Delete(
            Deletable deletable, 
            CancellationToken cancellationToken = default
        )
        {
            var memoryDeletable = CastAndValidate(
                deletable,
                d => (!string.IsNullOrWhiteSpace(d.MessageId), nameof(d.MessageId))
            );
            _messages.TryRemove(memoryDeletable.MessageId, out _);
            return Task.FromResult(new DeleteResponse());
        }

        /// <inheritdoc />
        public override Task<DispatchResponse> Dispatch(Message message, CancellationToken cancellationToken = default)
        {
            _messages.TryAdd(message.Id.ToString(), message);
            return Task.FromResult(new DispatchResponse {MessageId = message.Id});
        }

        /// <summary>
        /// Gets whether or not the memory provider contains the message.
        /// </summary>
        /// <param name="id">The id of the message.</param>
        public bool HasMessage(Guid id) => _messages.ContainsKey(id.ToString());

        /// <summary>
        /// Gets whether there are any messages in the memory provider.
        /// </summary>
        public bool HasMessages => _messages.Any();

        /// <inheritdoc />
        public override async Task<ReceiveResponse> Receive(
            Receivable receivable,
            CancellationToken cancellationToken = default
        ) => receivable.SecondsToWait != null ? await LongPoll(receivable) : GetMessages(receivable);

        private async Task<ReceiveResponse> LongPoll(Receivable receivable)
        {
            var messagesReceived = new List<ReceivedMessage>();
            // ReSharper disable once PossibleInvalidOperationException
            var timeToWaitUntil = DateTime.Now.AddSeconds(receivable.SecondsToWait.Value);

            bool ShouldContinuePolling() => messagesReceived.Count < receivable.MessagesToReceive &&
                DateTime.Now <= timeToWaitUntil;

            while (ShouldContinuePolling())
            {
                messagesReceived.AddRange(GetMessages(receivable).Messages);
                if (ShouldContinuePolling())
                    await Task.Delay(250);
            }

            return new ReceiveResponse {Messages = messagesReceived};
        }

        private ReceiveResponse GetMessages(Receivable receivable)
        {
            var messages = _messages
                .Where(message => message.Value.Expiry == null || message.Value.Expiry > DateTime.Now)
                .Take(receivable.MessagesToReceive)
                .Select(message => new ReceivedMessage
                {
                    Id = message.Value.Id,
                    Content = message.Value.Content,
                    Deletable = new MemoryDeletable { Queue = receivable.Queue, MessageId = message.Value.Id.ToString() },
                    Queue = message.Value.Queue
                });

            return new ReceiveResponse {Messages = messages};
        }
    }
}
