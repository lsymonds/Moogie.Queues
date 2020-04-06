namespace Moogie.Queues
{
    /// <summary>
    /// A collection of extensions methods that provide a fluent interface to the <see cref="Receivable"/> class.
    /// </summary>
    public static class ReceivableExtensions
    {
        /// <summary>
        /// Extension method for an integer that converts it into a <see cref="Receivable"/> object with the messages
        /// to receive set.
        /// </summary>
        /// <param name="messagesToReceive">The number of messages to receive. Should be 1.</param>
        /// <returns>The configured <see cref="Receivable"/> instance.</returns>
        public static Receivable Message(this int messagesToReceive) => messagesToReceive.Messages();

        /// <summary>
        /// Extension method for an integer that converts it into a <see cref="Receivable"/> object with the messages
        /// to receive set.
        /// </summary>
        /// <param name="messagesToReceive">The number of messages to receive. Should be greater than 1.</param>
        /// <returns>The configured <see cref="Receivable"/> instance.</returns>
        public static Receivable Messages(this int messagesToReceive) => new Receivable
        {
            MessagesToReceive = messagesToReceive
        };

        /// <summary>
        /// Configures a <see cref="Receivable"/> to receive messages from a certain queue.
        /// </summary>
        /// <param name="receivable">The <see cref="Receivable"/> to configure.</param>
        /// <param name="queue">The queue to receive messages from.</param>
        /// <returns>The <see cref="Receivable"/> instance with the Queue set.</returns>
        public static Receivable FromQueue(this Receivable receivable, string queue)
        {
            receivable.Queue = queue;
            return receivable;
        }

        /// <summary>
        /// Configures the Receive method to long poll and wait for the configured number of seconds.
        /// </summary>
        /// <param name="receivable">The <see cref="Receivable"/> to configure.</param>
        /// <param name="seconds">The seconds to wait for.</param>
        /// <returns>The <see cref="Receivable"/> instance with the number of seconds to wait set.</returns>
        public static Receivable ButWaitFor(this Receivable receivable, ushort seconds)
        {
            receivable.SecondsToWait = seconds;
            return receivable;
        }
    }
}
