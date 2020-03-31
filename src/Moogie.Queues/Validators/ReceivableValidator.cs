using System;

namespace Moogie.Queues.Validators
{
    internal static class ReceivableValidator
    {
        public static void Validate(Receivable receivable)
        {
            if (receivable == null)
                throw new ArgumentNullException(nameof(receivable));

            if (string.IsNullOrWhiteSpace(receivable.Queue))
                throw new MissingQueueException("Receivable");

            if (receivable.MessagesToReceive == 0)
                throw new InvalidMessagesToReceiveParameter();
        }
    }
}
