using System;

namespace Moogie.Queues
{
    /// <summary>
    /// A representation of a message that can be dispatched on a queue.
    /// </summary>
    public class Dispatchable : IProviderDispatchable
    {
        private Dispatchable()
        {
        }

        /// <inheritdoc />
        public Guid? Id { get; internal set; }

        /// <summary>
        /// Gets or sets the queue to dispatch the message on.
        /// </summary>
        public string Queue { get; private set; } = null!;

        /// <inheritdoc />
        public string? Content { get; internal set; }

        /// <inheritdoc />
        public DateTime? Expiry { get; internal set; }

        /// <summary>
        /// Entry point to the dispatchable class which yields a <see cref="Dispatchable"/> instance with the queue
        /// name configured.
        /// </summary>
        /// <param name="queue">The queue to dispatch on.</param>
        /// <returns>The configured <see cref="Dispatchable"/> instance.</returns>
        public static Dispatchable OnQueue(string queue) => new Dispatchable
        {
            Queue = queue
        };
    }
}
