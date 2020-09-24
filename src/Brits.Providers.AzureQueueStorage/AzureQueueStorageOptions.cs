namespace Brits
{
    /// <summary>
    /// Options for configuring the AzureQueueStorageProvider.
    /// </summary>
    public class AzureQueueStorageOptions
    {
        /// <summary>
        /// Gets or sets the connection string used to connect to the Azure Queue Storage queue.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets whether to ignore the long polling exception and continue on as normal.
        /// </summary>
        public bool IgnoreLongPollingException { get; set; }

        /// <summary>
        /// Gets or sets the queue name to interact with. If you want to interact with multiple Azure Queue Storage 
        /// queues, then you should instantiate multiple named providers and pass them to the UseBrits extension
        /// method.
        /// </summary>
        public string QueueName { get; set; }
    }
}
