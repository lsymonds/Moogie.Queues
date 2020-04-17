using System.Collections.Generic;

namespace Moogie.Queues
{
    /// <summary>
    /// Represents something which can be deleted.
    /// </summary>
    public class Deletable
    {
        /// <summary>
        /// Gets or sets the attributes used when deleting a message from a queue provider.
        /// </summary>
        internal Dictionary<string, string> DeletionAttributes { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// The queue to delete the message from.
        /// </summary>
        /// <value></value>
        public string Queue { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Deletable" /> class with a particular queue.
        /// </summary>
        /// <param name="queue">The queue to configure the <see cref="Deletable" /> instance with.</param>
        /// <returns>The configured <see cref="Deletable" /> instance.</returns>
        public static Deletable OffOfQueue(string queue) => new Deletable
        {
            Queue = queue
        };
    }
}
