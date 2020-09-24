using System;

namespace Brits
{
    /// <summary>
    /// Exception that is thrown when a consumer of the library attempts to add more than one queue with the same name
    /// to the collection of registered queue providers.
    /// </summary>
    public class DuplicateQueueException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="DuplicateQueueException"/> class.
        /// </summary>
        /// <param name="queue">The name of the queue that was to be added.</param>
        public DuplicateQueueException(string queue) : base(
            $"An attempt was made to add a duplicate queue with a name of {queue}.")
        {
        }
    }
}
