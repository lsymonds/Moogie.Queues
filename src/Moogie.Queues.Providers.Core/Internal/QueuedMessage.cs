using System;

namespace Moogie.Queues.Internal
{
    /// <summary>
    /// A message that has been queued and entered into the provider.
    /// </summary>
    public class QueuedMessage
    {
        /// <summary>
        /// Gets or sets the id of the queued message.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the queue the message was sent on.
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// Gets or sets the content of the queued message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets when the message should expire and subsequently be removed.
        /// </summary>
        public DateTime? Expiry { get; set; }
    }
}
