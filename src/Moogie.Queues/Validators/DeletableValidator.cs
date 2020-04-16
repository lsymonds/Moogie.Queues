using System;

namespace Moogie.Queues.Validators
{
    internal static class DeletableValidator
    {
        public static void Validate(Deletable deletable)
        {
            if (deletable == null)
                throw new ArgumentNullException(nameof(deletable));

            if (deletable.DeletionAttributes == null || deletable.DeletionAttributes.Count == 0)
                throw new ArgumentNullException(nameof(deletable.DeletionAttributes));

            if (string.IsNullOrWhiteSpace(deletable.Queue))
                throw new MissingQueueException("Deletable");
        }
    }
}
