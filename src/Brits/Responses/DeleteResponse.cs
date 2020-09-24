namespace Brits
{
    /// <summary>
    /// The response object returned from the Delete method on queue providers and managers.
    /// </summary>
    public class DeleteResponse
    {
        /// <summary>
        /// Gets or sets whether the deletion of the message succeeded.
        /// </summary>
        public bool Success { get; set; }
    }
}
