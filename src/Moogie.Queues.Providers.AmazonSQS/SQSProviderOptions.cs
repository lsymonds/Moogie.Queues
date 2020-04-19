using Amazon.Runtime;
using Amazon.SQS;

namespace Moogie.Queues
{
    /// <summary>
    /// Options to configure the <see cref="SQSProvider"/>.
    /// </summary>
    public class SQSProviderOptions
    {
        /// <summary>
        /// Gets or sets the options used to configure the AWS SQS SDK.
        /// </summary>
        public AmazonSQSConfig ClientConfig { get; set; }

        /// <summary>
        /// Gets or sets the credentials used to interact with SQS.
        /// </summary>
        public AWSCredentials Credentials { get; set; }

        /// <summary>
        /// Gets or sets the URL of the queue to send, receive and delete messages on/off.
        /// </summary>
        public string QueueUrl { get; set; }
    }
}
