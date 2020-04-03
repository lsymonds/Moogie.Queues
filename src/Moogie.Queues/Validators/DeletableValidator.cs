using System;

namespace Moogie.Queues.Validators
{
    internal static class DeletableValidator
    {
        public static void Validate(Deletable deletable)
        {
            if (deletable == null)
                throw new ArgumentNullException(nameof(deletable));

            if (deletable.Id == Guid.Empty)
                throw new ArgumentException(nameof(deletable.Id));

            if (string.IsNullOrWhiteSpace(deletable.Queue))
                throw new MissingQueueException("Deletable");
        }
    }
}
