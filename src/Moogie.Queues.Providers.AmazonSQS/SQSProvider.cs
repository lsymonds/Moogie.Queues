using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SQS;

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
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<ReceiveResponse> Receive(Receivable receivable)
        {
            throw new System.NotImplementedException();
        }
    }
}
