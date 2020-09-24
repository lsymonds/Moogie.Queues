using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Brits.Internal;

namespace Brits
{
    /// <summary>
    /// Brits provider for Azure's Queue Storage.
    /// </summary>
    public class AzureQueueStorageProvider : BaseProvider<AzureQueueStorageDeletable>
    {
        private readonly AzureQueueStorageOptions _options;
        private readonly QueueClient _azureQueueClient;

        /// <inheritdoc />
        public override string ProviderName { get; } = nameof(AzureQueueStorageProvider);

        /// <summary>
        /// Initialises a new instance of the <see cref="AzureQueueStorageProvider" /> class.
        /// </summary>
        /// <param name="options">The options used to configure the provider.</param>
        public AzureQueueStorageProvider(AzureQueueStorageOptions options)
        {
            _options = options;
            _azureQueueClient = new QueueClient(_options.ConnectionString, _options.QueueName);
        }

        /// <inheritdoc />
        public override async Task<DeleteResponse> Delete(
            Deletable deletable,
            CancellationToken cancellationToken = default
        )
        {
            var azureDeletable = CastAndValidate(
                deletable,
                d => (!string.IsNullOrWhiteSpace(d.MessageId), nameof(d.MessageId)),
                d => (!string.IsNullOrWhiteSpace(d.PopId), nameof(d.PopId))
            );
            
            var response = await _azureQueueClient
                .DeleteMessageAsync(azureDeletable.MessageId, azureDeletable.PopId, cancellationToken)
                .ConfigureAwait(false);
            
            return new DeleteResponse {Success = response.Status == 204};
        }

        /// <inheritdoc />
        public override async Task<DispatchResponse> Dispatch(
            Message message,
            CancellationToken cancellationToken = default
        )
        {
            var serialisedMessage = await message.Serialise(cancellationToken);

            await _azureQueueClient
                .SendMessageAsync(serialisedMessage, cancellationToken)
                .ConfigureAwait(false);

            return new DispatchResponse {MessageId = message.Id};
        }

        /// <inheritdoc />
        public override async Task<ReceiveResponse> Receive(
            Receivable receivable, 
            CancellationToken cancellationToken = default
        )
        {
            if (receivable.SecondsToWait != null && !_options.IgnoreLongPollingException)
                throw new FeatureNotYetSupportedException("AzureQueueStorageProvider: Long polling");

            var messages = await _azureQueueClient
                .ReceiveMessagesAsync(receivable.MessagesToReceive, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            var messagesToReturn = new List<ReceivedMessage>();
            foreach (var message in messages.Value)
            {
                var deletable = new AzureQueueStorageDeletable
                {
                    Queue = receivable.Queue,
                    MessageId = message.MessageId,
                    PopId = message.PopReceipt
                };

                var handledMessage = await DeserialiseAndHandle(message.MessageText, deletable, cancellationToken)
                    .ConfigureAwait(false);
                if (handledMessage != null)
                    messagesToReturn.Add(handledMessage);
            }
            
            return new ReceiveResponse { Messages = messagesToReturn };
        }
    }
}
