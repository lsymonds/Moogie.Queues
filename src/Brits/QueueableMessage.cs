using System;

namespace Brits
{
    /// <summary>
    /// Base class that represents a message that can be entered into a queue.
    /// </summary>
    public abstract class QueueableMessage
    {
        /// <summary>
        /// Gets or sets the id of the message.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content of the message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets when the message expires.
        /// </summary>
        public DateTime? Expiry { get; set; }

        /// <summary>
        /// Gets or sets the queue associated with the message.
        /// </summary>
        public string Queue { get; set; } = "default";
    }
}
