using System.Collections.Generic;

namespace Moogie.Queues
{
    /// <summary>
    /// Represents something which can be deleted.
    /// </summary>
    public class Deletable
    {
        public IReadOnlyDictionary<string, string> DeletionAttributes { get; set; }

        public string Queue { get; set; }
    }
}
