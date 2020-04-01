namespace Moogie.Queues
{
    /// <summary>
    /// A collection of extensions methods that provide a fluent interface to the <see cref="Receivable"/> class.
    /// </summary>
    public static class ReceivableExtensions
    {
        public static Receivable Message(this int messagesToReceive) => messagesToReceive.Messages();

        public static Receivable Messages(this int messagesToReceive) => new Receivable
        {
            MessagesToReceive = messagesToReceive
        };

        public static Receivable FromQueue(this Receivable receivable, string queue)
        {
            receivable.Queue = queue;
            return receivable;
        }

        /// <summary>
        /// Sets the amount of messages to receive from the Receive request. Defaults to 1.
        /// </summary>
        /// <param name="receivable">The <see cref="Receivable"/> instance to modify.</param>
        /// <param name="amount">The amount of messages to receive.</param>
        /// <returns>The modified <see cref="Receivable"/> instance.</returns>
        public static Receivable AmountOfMessagesToReceive(this Receivable receivable, int amount)
        {
            receivable.MessagesToReceive = amount;
            return receivable;
        }
    }
}
