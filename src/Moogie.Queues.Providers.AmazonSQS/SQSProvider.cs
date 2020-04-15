using System.Collections.Generic;
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
    public class SQSProvider : BaseProvider
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
        public override async Task<DeleteResponse> Delete(Deletable deletable)
        {
            var response = await _client.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = _options.QueueUrl,
                ReceiptHandle = deletable.ReceiptHandle
            });

            return new DeleteResponse { Success = response != null && (int)response.HttpStatusCode == 200 };
        }

        /// <inheritdoc />
        public override async Task<DispatchResponse> Dispatch(Message message)
        {
            await _client.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = _options.QueueUrl,
                MessageBody = await ((QueueableMessage)message).Serialise().ConfigureAwait(false)
            }).ConfigureAwait(false);

            return new DispatchResponse { MessageId = message.Id };
        }

        /// <inheritdoc />
        public override async Task<ReceiveResponse> Receive(Receivable receivable)
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
                var deserialised = await DeserialiseAndHandle(message.Body, message.ReceiptHandle, receivable.Queue);
                if (deserialised != null)
                    messagesToReturn.Add(deserialised);
            }

            return new ReceiveResponse
            {
                Messages = messagesToReturn
            };
        }
    }
}
