using System;

namespace Moogie.Queues.Validators
{
    internal static class MessageValidator
    {
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
