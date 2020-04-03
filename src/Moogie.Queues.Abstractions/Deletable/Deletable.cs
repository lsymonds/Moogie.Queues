using System;

namespace Moogie.Queues
{
    /// <summary>
    /// Represents something which can be deleted.
    /// </summary>
    public class Deletable
    {
        /// <summary>
        /// Gets or sets the id of the deletable object.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the queue to delete the message from.
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// Creates a <see cref="Deletable"/> instance from an id.
        /// </summary>
        /// <param name="id">The id of the message to delete.</param>
        /// <returns>A <see cref="Deletable"/> instance.</returns>
        public static Deletable FromId(Guid id) => new Deletable
        {
            Id = id
        };
    }
}
