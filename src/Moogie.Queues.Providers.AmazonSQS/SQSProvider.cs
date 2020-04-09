using System;
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

            var credentials  = options.Credentials ?? FallbackCredentialsFactory.GetCredentials();
            var config = options.ClientConfig ?? new AmazonSQSConfig();

            _client = new AmazonSQSClient(credentials, config);
        }

        /// <inheritdoc />
        public Task<DeleteResponse> Delete(Deletable deletable)
        {
            throw new System.NotImplementedException();
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

            return new DispatchResponse {MessageId = messageToQueue.Id};
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

            // Throw exception if not successful.
            // Loop through, deleting any messages that have expired.
            // Return the response.

            throw new ArgumentNullException();
        }
    }
}
