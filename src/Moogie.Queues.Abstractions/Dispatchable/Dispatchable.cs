using System;

namespace Moogie.Queues
{
    /// <summary>
    /// A representation of a message that can be dispatched on a queue.
    /// </summary>
    public class Dispatchable : IProviderDispatchable
    {
        /// <inheritdoc />
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the queue to dispatch the message on.
        /// </summary>
        public string Queue { get; set; }

        /// <inheritdoc />
        public string? Content { get; set; }

        /// <inheritdoc />
        public DateTime? Expiry { get; set; }
    }
}
