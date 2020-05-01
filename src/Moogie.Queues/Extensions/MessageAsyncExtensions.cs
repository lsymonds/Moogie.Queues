using System;
using System.Threading.Tasks;

namespace Moogie.Queues
{
    /// <summary>
    /// A collection of extension methods that provide a fluent interface to modifying the <see cref="Message"/>
    /// class when the async Message.WithContent entry point is used.
    /// </summary>
    public static class MessageAsyncExtensions
    {
        /// <summary>
        /// Sets the Id of the <see cref="Message"/> instance.
        /// </summary>
        /// <param name="messageTask">The task of the <see cref="Message"/> instance to modify.</param>
        /// <param name="id">The id to set.</param>
        /// <returns>The asynchronous task of the modified <see cref="Message"/> instance.</returns>
        public static async Task<Message> WithId(this Task<Message> messageTask, Guid id)
        {
            var message = await messageTask;
            return message.WithId(id);
        }

        /// <summary>
        /// Sets the Queue of the <see cref="Message" /> instance.
        /// </summary>
        /// <param name="messageTask">The task of the <see cref="Message"/> instance to modify.</param>
        /// <param name="queue">The queue to set.</param>
        /// <returns>The asynchronous task of the modified <see cref="Message"/> instance.</returns>
        public static async Task<Message> OnQueue(this Task<Message> messageTask, string queue)
        {
            var message = await messageTask;
            return message.OnQueue(queue);
        }

        /// <summary>
        /// Sets the Expiry property of the <see cref="Message"/> instance.
        /// </summary>
        /// <param name="messageTask">The task of the <see cref="Message"/> instance to modify.</param>
        /// <param name="expiry">The expiry date of the message.</param>
        /// <returns>The asynchronous task of the modified <see cref="Message"/> instance.</returns>
        public static async Task<Message> WhichExpiresAt(this Task<Message> messageTask, DateTime expiry)
        {
            var message = await messageTask;
            return message.WhichExpiresAt(expiry);
        }
    }
}