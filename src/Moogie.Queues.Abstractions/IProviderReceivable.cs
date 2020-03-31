namespace Moogie.Queues
{
    /// <summary>
    /// An interface defining required properties for provider Receive methods.
    /// </summary>
    public interface IProviderReceivable
    {
        /// <summary>
        /// Gets or sets the amount of messages to receive.
        /// </summary>
        uint MessagesToReceive { get; }
    }
}
