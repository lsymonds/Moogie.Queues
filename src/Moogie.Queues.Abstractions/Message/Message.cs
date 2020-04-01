using System;

namespace Moogie.Queues
{
    /// <summary>
    /// A representation of a message that can be dispatched on a queue.
    /// </summary>
    public class Message
    {
        /// <inheritdoc />
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the queue to dispatch the message on.
        /// </summary>
        public string Queue { get; set; } = null!;

        /// <inheritdoc />
        public string? Content { get; set; }

        /// <inheritdoc />
        public DateTime? Expiry { get; set; }

        /// <summary>
        /// Fluent entry point to the message class which yields a <see cref="Message"/> instance with the queue
        /// name configured.
        /// </summary>
        /// <param name="queue">The queue to dispatch on.</param>
        /// <returns>The configured <see cref="Message"/> instance.</returns>
        public static Message OnQueue(string queue) => new Message
        {
            Queue = queue
        };
    }
}
