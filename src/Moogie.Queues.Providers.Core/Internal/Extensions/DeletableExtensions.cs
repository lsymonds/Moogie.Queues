namespace Moogie.Queues.Internal
{
    /// <summary>
    /// Wrapper for internal extensions that modify a <see cref="Deletable" /> instance.
    /// </summary>
    public static class DeletableExtensions
    {
        /// <summary>
        /// Adds a deletion attribute to the deletion attributes dictionary on a <see cref="Deletable" /> instance.
        /// </summary>
        /// <param name="deletable">The deletable to modify.</param>
        /// <param name="attribute">The attribute to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The modified deletable instance.</returns>
        public static Deletable WithDeletionAttribute(this Deletable deletable, string attribute, string value)
        {
            deletable.DeletionAttributes.Add(attribute, value);
            return deletable;
        }
    }
}