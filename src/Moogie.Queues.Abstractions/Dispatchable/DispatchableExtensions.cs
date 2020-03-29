using System;

namespace Moogie.Queues
{
    /// <summary>
    /// A collection of extension methods that provide a fluent interface to the <see cref="Dispatchable"/> class.
    /// </summary>
    public static class DispatchableExtensions
    {
        /// <summary>
        /// Sets the Id of the <see cref="Dispatchable"/> instance.
        /// </summary>
        /// <param name="dispatchable">The <see cref="Dispatchable"/> instance to modify.</param>
        /// <param name="id">The id to set.</param>
        /// <returns>The modified <see cref="Dispatchable"/> instance.</returns>
        public static Dispatchable WithId(this Dispatchable dispatchable, Guid id)
        {
            dispatchable.Id = id;
            return dispatchable;
        }

        /// <summary>
        /// Sets the Queue property of the <see cref="Dispatchable"/> instance.
        /// </summary>
        /// <param name="dispatchable">The <see cref="Dispatchable"/> instance to modify.</param>
        /// <param name="queue">The queue to dispatch the message on.</param>
        /// <returns>The modified <see cref="Dispatchable"/> instance.</returns>
        public static Dispatchable OnQueue(this Dispatchable dispatchable, string queue)
        {
            dispatchable.Queue = queue;
            return dispatchable;
        }

        /// <summary>
        /// Sets the Content property of the <see cref="Dispatchable"/> instance.
        /// </summary>
        /// <param name="dispatchable">The <see cref="Dispatchable"/> instance to modify.</param>
        /// <param name="content">The content to include in the message.</param>
        /// <returns>The modified <see cref="Dispatchable"/> instance.</returns>
        public static Dispatchable WithContent(this Dispatchable dispatchable, string content)
        {
            dispatchable.Content = content;
            return dispatchable;
        }

        /// <summary>
        /// Sets the Expiry property of the <see cref="Dispatchable"/> instance.
        /// </summary>
        /// <param name="dispatchable">The <see cref="Dispatchable"/> instance to modify.</param>
        /// <param name="expiry">The expiry date of the message.</param>
        /// <returns>The modified <see cref="Dispatchable"/> instance.</returns>
        public static Dispatchable ExpireAt(this Dispatchable dispatchable, DateTime expiry)
        {
            dispatchable.Expiry = expiry;
            return dispatchable;
        }
    }
}
