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
        /// <param name="message">The message to dispatch onto the queue.</param>
        /// <returns>The response from the Dispatch command.</returns>
        Task<DispatchResponse> Dispatch(Message message);

        /// <summary>
        /// Receives a message or messages from the queue.
        /// </summary>
        /// <param name="receivable">The object used to configure the receive message on the provider.</param>
        /// <returns>An object containing the received messages.</returns>
        Task<ReceiveResponse> Receive(Receivable receivable);
    }
}
