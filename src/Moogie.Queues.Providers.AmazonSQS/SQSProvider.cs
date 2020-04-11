using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Moogie.Queues.Internal;

namespace Moogie.Queues.Providers.AmazonSQS
{
    /// <summary>
    /// Moogie.Queues provider for Amazon's Simple Queue Service.
    /// </summary>
    public class SQSProvider : IQueueProvider
    {
        private readonly SQSOptions _options;
        private readonly AmazonSQSClient _client;

        /// <summary>
        /// Initialises a new instance of the <see cref="SQSProvider"/> class with the configured AWS options.
        /// </summary>
        /// <param name="options">The options to configure the provider with.</param>
        public SQSProvider(SQSOptions options)
        {
            _options = options;

            var credentials = options.Credentials ?? FallbackCredentialsFactory.GetCredentials();
            var config = options.ClientConfig ?? new AmazonSQSConfig();

            _client = new AmazonSQSClient(credentials, config);
        }

        /// <inheritdoc />
        public async Task<DeleteResponse> Delete(Deletable deletable)
        {
            return null;
        }

        /// <inheritdoc />
        public async Task<DispatchResponse> Dispatch(Message message)
        {
            var messageToQueue = new QueuedMessage
            {
                Id = message.Id ?? Guid.NewGuid(),
                Queue = message.Queue,
                Content = message.Content,
                Expiry = message.Expiry
            };

            await _client.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = _options.QueueUrl,
                MessageBody = await messageToQueue.Serialise().ConfigureAwait(false)
            }).ConfigureAwait(false);

            return new DispatchResponse { MessageId = messageToQueue.Id };
        }

        /// <inheritdoc />
        public async Task<ReceiveResponse> Receive(Receivable receivable)
        {
            var messages = await _client.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = _options.QueueUrl,
                WaitTimeSeconds = receivable.SecondsToWait ?? 0,
                MaxNumberOfMessages = receivable.MessagesToReceive
            });

            var messagesToReturn = new List<ReceivedMessage>();
            foreach (var message in messages.Messages)
            {
                var deserialised = await message.Body.TryDeserialise<QueuedMessage>();
                if (deserialised == null)
                    continue;

                if (deserialised.Expiry != null && deserialised.Expiry < DateTime.Now)
                {
                    await Delete(Deletable.WithReceiptHandle(message.ReceiptHandle).OnQueue(receivable.Queue));
                    continue;
                }

                messagesToReturn.Add(new ReceivedMessage
                {
                    Id = deserialised.Id,
                    Queue = deserialised.Queue,
                    Content = deserialised.Content,
                    ReceiptHandle = message.ReceiptHandle
                });
            }

            return new ReceiveResponse
            {
                Messages = messagesToReturn
            };
        }
    }
}
