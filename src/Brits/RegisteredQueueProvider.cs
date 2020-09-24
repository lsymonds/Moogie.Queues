namespace Brits
{
    /// <summary>
    /// Represents a provider that has been registered with a queue manager instance.
    /// </summary>
    public class RegisteredQueueProvider
    {
        /// <summary>
        /// Gets or sets the name that the queue provider has been registered under. 
        /// </summary>
        public string Name { get; internal set; }
        
        /// <summary>
        /// Gets or sets the name of the provider that has been registered, for example: SQSProvider.
        /// </summary>
        public string ProviderName { get; internal set; }
    }
}
