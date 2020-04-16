using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Moogie.Queues.Internal;

namespace Moogie.Queues
{
    /// <summary>
    /// Moogie.Queues provider for Azure's Queue Storage.
    /// </summary>
    public class AzureQueueStorageProvider : BaseProvider
    {
        private readonly AzureQueueStorageOptions _options;
        private readonly QueueClient _azureQueueClient; 

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
        public override Task<DeleteResponse> Delete(Deletable deletable)
        {
        }

        /// <inheritdoc />
        public override async Task<DispatchResponse> Dispatch(Message message)
        {
            var timeToLive = message.Expiry != null ? message.Expiry - DateTime.Now : null;
            var response = await _azureQueueClient.SendMessageAsync(await message.Serialise()).ConfigureAwait(false);
            
            return new DispatchResponse { MessageId = message.Id };
        }

        /// <inheritdoc />
        public override async Task<ReceiveResponse> Receive(Receivable receivable)
        {
            if (receivable.SecondsToWait != null)
                throw new FeatureNotYetSupportedException("AzureQueueStorageProvider: Long polling");

            var messages = await _azureQueueClient.ReceiveMessagesAsync(receivable.MessagesToReceive).ConfigureAwait(false);

            var messagesToReturn = new List<ReceivedMessage>();
            foreach (var message in messages.Value)
            {
                var handledMessage = await DeserialiseAndHandle(message.MessageText, message.MessageId, receivable.Queue).ConfigureAwait(false);
                if (handledMessage != null)
                    messagesToReturn.Add(handledMessage);
            }
            
            return new ReceiveResponse { Messages = messagesToReturn };
        }
    }
}