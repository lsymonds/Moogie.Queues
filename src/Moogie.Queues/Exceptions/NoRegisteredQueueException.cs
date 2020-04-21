using System;

namespace Moogie.Queues
{
    /// <summary>
    /// Exception that is thrown when an attempt is made to either dispatch a message on or receive a message from
    /// a non-registered queue.
    /// </summary>
    public class NoRegisteredQueueException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="NoRegisteredQueueException"/> class.
        /// </summary>
        /// <param name="queue">The queue that the attempt to dispatch or receive against was made.</param>
        public NoRegisteredQueueException(string queue) : base($"No registered queue with a name of {queue} was found.")
        {
        }
    }
}
