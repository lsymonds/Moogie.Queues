using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moogie.Queues.Internal;

namespace Moogie.Queues
{
    /// <summary>
    /// The response object returned from the Receive method on queue providers and managers.
    /// </summary>
    public class ReceiveResponse
    {
        /// <summary>
        /// Gets or sets the messages that were received.
        /// </summary>
        public IEnumerable<ReceivedMessage> Messages { get; set; }
    }

    /// <summary>
    /// A message that is received from a queue.
    /// </summary>
    public class ReceivedMessage : QueueableMessage
    {
        /// <summary>
        /// Gets or sets the object used to delete this message from the queue.
        /// </summary>
        public Deletable Deletable { get; set; }

        /// <summary>
        /// Reads the <see cref="Content"/> property as an object.
        /// </summary>
        /// <param name="cancellationToken">The token used to cancel the serialisation.</param>
        /// <typeparam name="T">The type to deserialise the message into.</typeparam>
        /// <returns>An asynchronous <see cref="Task"/> yielding the deserialised content.</returns>
        public async Task<T> ReadContentAs<T>(CancellationToken cancellationToken = default)
            => await Content.TryDeserialise<T>(cancellationToken);
    }
}
