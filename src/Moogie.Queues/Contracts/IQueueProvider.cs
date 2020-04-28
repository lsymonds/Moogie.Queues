using System.Threading;
using System.Threading.Tasks;

namespace Moogie.Queues
{
    /// <summary>
    /// An interface defining what each queue provider must implement.
    /// </summary>
    public interface IQueueProvider
    {
        /// <summary>
        /// Gets the name of the provider (i.e. AmazonSQS).
        /// </summary>
        string ProviderName { get; }
        
        /// <summary>
        /// Delete a message from the queue. Also known as confirming a message in some queue systems.
        /// </summary>
        /// <param name="deletable">The representation of a message to delete.</param>
        /// <param name="cancellationToken">The optional cancellation token to use in asynchronous operations.</param>
        /// <returns>The response from the delete or confirm of the message.</returns>
        Task<DeleteResponse> Delete(Deletable deletable, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dispatches a message onto the queue.
        /// </summary>
        /// <param name="message">The message to dispatch onto the queue.</param>
        /// <param name="cancellationToken">The optional cancellation token to use in asynchronous operations.</param>
        /// <returns>The response from the Dispatch command.</returns>
        Task<DispatchResponse> Dispatch(Message message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Receives a message or messages from the queue.
        /// </summary>
        /// <param name="receivable">The object used to configure the receive message on the provider.</param>
        /// <param name="cancellationToken">The optional cancellation token to use in asynchronous operations.</param>
        /// <returns>An object containing the received messages.</returns>
        Task<ReceiveResponse> Receive(Receivable receivable, CancellationToken cancellationToken = default);
    }
}
