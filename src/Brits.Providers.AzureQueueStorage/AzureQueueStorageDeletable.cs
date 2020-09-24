namespace Brits
{
    /// <summary>
    /// Deletable implementation for the Azure queue storage provider.
    /// </summary>
    public class AzureQueueStorageDeletable : Deletable
    {
        /// <summary>
        /// Gets the id of the message to delete.
        /// </summary>
        public string MessageId { get; set;}
        
        /// <summary>
        /// Gets the pop id of the message to delete.
        /// </summary>
        public string PopId { get; set; }
    }
}
