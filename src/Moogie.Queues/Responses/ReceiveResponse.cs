using System.Collections.Generic;

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
    }
}
