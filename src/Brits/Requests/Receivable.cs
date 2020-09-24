namespace Brits
{
    /// <summary>
    /// Configuration options for receiving messages from a queue.
    /// </summary>
    public class Receivable
    {
        /// <summary>
        /// Gets or sets the queue to receive the messages from.
        /// </summary>
        public string Queue { get; set; } = "default";

        /// <summary>
        /// Gets or sets the number of messages to receive.
        /// </summary>
        public int MessagesToReceive { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of seconds to wait when long polling until the requested number of messages are
        /// received.. If this property is left as null, then long polling is disabled.
        /// </summary>
        public ushort? SecondsToWait { get; set; }
    }
}
