using System;

namespace Moogie.Queues.Validators
{
    /// <summary>
    /// Wrapper class that contains <see cref="Deletable" /> validation methods.
    /// </summary>
    internal static class DeletableValidator
    {
        /// <summary>
        /// Validates a <see cref="Deletable" /> instance.
        /// </summary>
        /// <param name="deletable">The <see cref="Deletable" /> instance to validate.</param>
        public static void Validate(Deletable deletable)
        {
            if (deletable == null)
                throw new ArgumentNullException(nameof(deletable));

            if (string.IsNullOrWhiteSpace(deletable.Queue))
                throw new MissingQueueException("Deletable");

            if (deletable.DeletionAttributes == null || deletable.DeletionAttributes.Count == 0)
                throw new ArgumentNullException(nameof(deletable.DeletionAttributes));
        }
    }
}
