using System;
using System.Collections.Generic;

namespace Moogie.Queues
{
    /// <summary>
    /// The response object returned from the Receive method on queue providers and managers.
    /// </summary>
    public class ReceiveResponse
    {
        /// <summary>
        /// A message that is received from a queue.
        /// </summary>
        public class ReceivedMessage
        {
            /// <summary>
            /// Gets or sets the id of the received message.
            /// </summary>
            public Guid Id { get; set; }

            /// <summary>
            /// Gets or sets the content of the received message.
            /// </summary>
            public string Content { get; set; }
        }

        /// <summary>
        /// Gets or sets the messages that were received.
        /// </summary>
        public IEnumerable<ReceivedMessage> Messages { get; set; }
    }
}
