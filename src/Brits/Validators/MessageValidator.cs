using System;

namespace Brits.Validators
{
    /// <summary>
    /// Wrapper class that contains <see cref="Message" /> validation methods.
    /// </summary>
    internal static class MessageValidator
    {
        /// <summary>
        /// Validates a <see cref="Message" /> instance.
        /// </summary>
        /// <param name="message">The <see cref="Message" /> instance to validate.</param>
        public static void Validate(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (string.IsNullOrWhiteSpace(message.Queue))
                throw new MissingQueueException(nameof(Message));

            if (string.IsNullOrWhiteSpace(message.Content))
                throw new MissingContentException();

            if (message.Expiry != null && message.Expiry <= DateTime.Now)
                throw new InvalidDispatchableExpiryException(message?.Expiry);
        }
    }
}
