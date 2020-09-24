using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Brits.Internal;

namespace Brits
{
    /// <summary>
    /// Brits provider for Amazon's Simple Queue Service.
    /// </summary>
    public class SQSProvider : BaseProvider<SQSDeletable>
    {
        private readonly SQSProviderOptions _options;
        private readonly AmazonSQSClient _client;

        /// <inheritdoc />
        public override string ProviderName { get; } = nameof(SQSProvider);

        /// <summary>
        /// Initialises a new instance of the <see cref="SQSProvider"/> class with the configured AWS options.
        /// </summary>
        /// <param name="options">The options to configure the provider with.</param>
        public SQSProvider(SQSProviderOptions options)
        {
            _options = options;

            var credentials = options.Credentials ?? FallbackCredentialsFactory.GetCredentials();
            var config = options.ClientConfig ?? new AmazonSQSConfig();

            _client = new AmazonSQSClient(credentials, config);
        }

        /// <inheritdoc />
        public override async Task<DeleteResponse> Delete(
            Deletable deletable,
            CancellationToken cancellationToken = default
        )
        {
            var sqsDeletable = CastAndValidate(
                deletable,
                d => (!string.IsNullOrWhiteSpace(d.ReceiptHandle), nameof(d.ReceiptHandle))
            );
            
            var response = await _client.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = _options.QueueUrl,
                ReceiptHandle = sqsDeletable.ReceiptHandle
            }, cancellationToken).ConfigureAwait(false);

            return new DeleteResponse {Success = response != null && (int) response.HttpStatusCode == 200};
        }

        /// <inheritdoc />
        public override async Task<DispatchResponse> Dispatch(
            Message message,
            CancellationToken cancellationToken = default
        )
        {
            await _client.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = _options.QueueUrl,
                MessageBody = await ((QueueableMessage) message).Serialise(cancellationToken).ConfigureAwait(false)
            }, cancellationToken).ConfigureAwait(false);

            return new DispatchResponse {MessageId = message.Id};
        }

        /// <inheritdoc />
        public override async Task<ReceiveResponse> Receive(
            Receivable receivable,
            CancellationToken cancellationToken = default
        )
        {
            if (receivable.MessagesToReceive > 10)
                receivable.MessagesToReceive = 10;

            var messages = await _client.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = _options.QueueUrl,
                WaitTimeSeconds = receivable.SecondsToWait ?? 0,
                MaxNumberOfMessages = receivable.MessagesToReceive
            }, cancellationToken).ConfigureAwait(false);

            var messagesToReturn = new List<ReceivedMessage>();
            foreach (var message in messages.Messages)
            {
                var deletable = new SQSDeletable { Queue = receivable.Queue, ReceiptHandle = message.ReceiptHandle };

                var deserialised = await DeserialiseAndHandle(message.Body, deletable, cancellationToken)
                    .ConfigureAwait(false);
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
