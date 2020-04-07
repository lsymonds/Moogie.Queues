using System;

namespace Moogie.Queues
{
    /// <summary>
    /// A collection of extension methods that provide a fluent interface to the <see cref="Message"/> class.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Sets the Id of the <see cref="Message"/> instance.
        /// </summary>
        /// <param name="message">The <see cref="Message"/> instance to modify.</param>
        /// <param name="id">The id to set.</param>
        /// <returns>The modified <see cref="Message"/> instance.</returns>
        public static Message WithId(this Message message, Guid id)
        {
            message.Id = id;
            return message;
        }

        /// <summary>
        /// Sets the Content property of the <see cref="Message"/> instance.
        /// </summary>
        /// <param name="message">The <see cref="Message"/> instance to modify.</param>
        /// <param name="content">The content to include in the message.</param>
        /// <returns>The modified <see cref="Message"/> instance.</returns>
        public static Message WithContent(this Message message, string content)
        {
            message.Content = content;
            return message;
        }

        /// <summary>
        /// Sets the Expiry property of the <see cref="Message"/> instance.
        /// </summary>
        /// <param name="message">The <see cref="Message"/> instance to modify.</param>
        /// <param name="expiry">The expiry date of the message.</param>
        /// <returns>The modified <see cref="Message"/> instance.</returns>
        public static Message WhichExpiresAt(this Message message, DateTime expiry)
        {
            message.Expiry = expiry;
            return message;
        }
    }
}
