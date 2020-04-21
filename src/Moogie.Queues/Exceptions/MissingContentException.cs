using System;

namespace Moogie.Queues
{
    /// <summary>
    /// Exception that is thrown when the Content property on a <see cref="Message"/> instance is invalid.
    /// </summary>
    public class MissingContentException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="MissingContentException"/> class.
        /// </summary>
        public MissingContentException() : base("Dispatchable object is missing its Content property.")
        {
        }
    }
}
