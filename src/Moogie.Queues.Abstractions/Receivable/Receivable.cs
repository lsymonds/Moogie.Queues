namespace Moogie.Queues
{
    /// <summary>
    /// Configuration options for receiving messages from a queue.
    /// </summary>
    public class Receivable : IProviderReceivable
    {
        private Receivable()
        {
        }

        /// <summary>
        /// Gets or sets the queue to receive the messages from.
        /// </summary>
        public string Queue { get; private set; } = null!;

        /// <inheritdoc />
        public uint MessagesToReceive { get; internal set; } = 1;

        /// <summary>
        /// Gets or sets the number of seconds to wait when long polling. If this property is left as null, then
        /// long polling is disabled.
        /// </summary>
        public ushort? SecondsToWait { get; internal set; }

        /// <summary>
        /// Entry point to the <see cref="Receivable"/> class which configures which queue to receive messages from.
        /// </summary>
        /// <param name="queue">The queue to receive messages from.</param>
        /// <returns>The configured <see cref="Receivable"/> instance.</returns>
        public static Receivable FromQueue(string queue) => new Receivable
        {
            Queue = queue
        };
    }
}
