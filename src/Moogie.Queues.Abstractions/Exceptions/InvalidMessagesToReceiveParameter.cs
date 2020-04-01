using System;

namespace Moogie.Queues
{
    /// <summary>
    /// Exception that is thrown when the MessagesToReceive parameter is set to zero or less than zero. I mean, why
    /// would you want to make a request and receive zero messages?!
    /// </summary>
    public class InvalidMessagesToReceiveParameter : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="InvalidMessagesToReceiveParameter"/> class.
        /// </summary>
        public InvalidMessagesToReceiveParameter() : base(
            "The MessagesToReceive parameter was zero or less than zero. Why would you want to receive zero messages?!")
        {
        }
    }
}
