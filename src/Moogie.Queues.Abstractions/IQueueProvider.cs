using System.Threading.Tasks;

namespace Moogie.Queues
{
    /// <summary>
    /// An interface defining what each queue provider must implement.
    /// </summary>
    public interface IQueueProvider
    {
        /// <summary>
        /// Dispatches a message onto the queue.
        /// </summary>
        /// <param name="dispatchable">The message to dispatch onto the queue.</param>
        /// <returns>The response from the Dispatch command.</returns>
        Task<DispatchResponse> Dispatch(IProviderDispatchable dispatchable);

        /// <summary>
        /// Receives a message or messages from the queue.
        /// </summary>
        /// <param name="receivable">The object used to configure the receive message on the provider.</param>
        /// <returns>An object containing the received messages.</returns>
        Task<ReceiveResponse> Receive(IProviderReceivable receivable);
    }
}
