namespace Brits
{
    /// <summary>
    /// Memory provider implementation of the <see cref="Deletable"/> entity.
    /// </summary>
    public class MemoryDeletable : Deletable
    {
        /// <summary>
        /// Gets or sets the id of the message to delete.
        /// </summary>
        public string MessageId { get; set; }
    }
}
