using System;

namespace Brits
{
    /// <summary>
    /// Exception that is thrown when a deletion attribute is missing.
    /// </summary>
    public class MissingDeletionAttributeException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="MissingDeletionAttributeException" /> class.
        /// </summary>
        /// <param name="attribute">The attribute that was missing.</param>
        public MissingDeletionAttributeException(string attribute) 
            : base($"The attribute {attribute} was missing from the deletion attributes property.")
        {
        }
    }
}
