using System;

namespace Moogie.Queues
{
    /// <summary>
    /// A collection of extension methods that provide a fluent interface to the <see cref="Deletable"/> class.
    /// </summary>
    public static class DeletableExtensions
    {
        /// <summary>
        /// Sets the Queue parameter of the <see cref="Deletable"/> instance.
        /// </summary>
        /// <param name="deletable">The <see cref="Deletable"/> instance to modify.</param>
        /// <param name="queue">The queue to delete the message from.</param>
        /// <returns>The modified <see cref="Deletable"/> instance with the queue set.</returns>
        public static Deletable OnQueue(this Deletable deletable, string queue)
        {
            deletable.Queue = queue;
            return deletable;
        }

        /// <summary>
        /// Converts a message into a <see cref="Deletable"/> object.
        /// </summary>
        /// <param name="message">The message to convert.</param>
        /// <returns>The converted <see cref="Deletable"/> instance.</returns>
        public static Deletable AsDeletable(this Message message) => new Deletable
        {
            Id = message?.Id ?? Guid.Empty,
            Queue = message?.Queue ?? null
        };

        /// <summary>
        /// Converts a ReceiveResponse object into a <see cref="Deletable"/> object.
        /// </summary>
        /// <param name="receiveResponse">The response to convert.</param>
        /// <returns>The converted <see cref="Deletable"/> object.</returns>
        public static Deletable AsDeletable(this ReceiveResponse receiveResponse) => new Deletable
        { };
    }
}
