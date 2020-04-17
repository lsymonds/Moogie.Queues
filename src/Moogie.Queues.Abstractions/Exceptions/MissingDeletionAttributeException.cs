using System;

namespace Moogie.Queues
{
    /// <summary>
    /// Exception that is thrown when a deletion attribute is missing.
    /// </summary>
    public class MissingDeletionAttributeException : Exception
    {

        public MissingDeletionAttributeException(string attribute) 
            : base($"The attribute {attribute} was missing from the deletion attributes property.")
        {
        }
    }
}