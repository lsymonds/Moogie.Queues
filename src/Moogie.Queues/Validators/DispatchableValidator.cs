using System;

namespace Moogie.Queues.Validators
{
    internal static class DispatchableValidator
    {
        public static void Validate(Dispatchable dispatchable)
        {
            if (dispatchable == null)
                throw new ArgumentNullException(nameof(dispatchable));

            if (string.IsNullOrWhiteSpace(dispatchable.Queue))
                throw new MissingQueueException(nameof(Dispatchable));

            if (string.IsNullOrWhiteSpace(dispatchable.Content))
                throw new MissingContentException();

            if (dispatchable.Expiry != null && dispatchable.Expiry <= DateTime.Now)
                throw new InvalidDispatchableExpiryException(dispatchable?.Expiry);
        }
    }
}
