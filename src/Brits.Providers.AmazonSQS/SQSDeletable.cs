namespace Brits
{
    /// <summary>
    /// Deletable implementation for the SQS provider.
    /// </summary>
    public class SQSDeletable : Deletable
    {
        /// <summary>
        /// Gets the receipt handle of the message to delete.
        /// </summary>
        public string ReceiptHandle { get; internal set; }
    }
}
