using System;

namespace Brits.Validators
{
    /// <summary>
    /// Wrapper class that contains <see cref="Receivable" /> validation methods.
    /// </summary>
    internal static class ReceivableValidator
    {
        /// <summary>
        /// Validates a <see cref="Receivable" /> instance.
        /// </summary>
        /// <param name="receivable">The <see cref="Receivable" /> instance to validate.</param>
        public static void Validate(Receivable receivable)
        {
            if (receivable == null)
                throw new ArgumentNullException(nameof(receivable));

            if (string.IsNullOrWhiteSpace(receivable.Queue))
                throw new MissingQueueException("Receivable");

            if (receivable.MessagesToReceive <= 0)
                throw new InvalidMessagesToReceiveParameterException();
        }
    }
}
