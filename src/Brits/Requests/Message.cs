using System.Threading;
using System.Threading.Tasks;
using Brits.Internal;

namespace Brits
{
    /// <summary>
    /// A representation of a message that can be dispatched on a queue.
    /// </summary>
    public class Message : QueueableMessage
    {
        /// <summary>
        /// Fluent entry point to the message class which yields a <see cref="Message"/> instance with the content
        /// configured.
        /// </summary>
        /// <param name="content">The content to send.</param>
        /// <returns>The configured <see cref="Message"/> instance.</returns>
        public static Message WithContent(string content) => new Message
        {
            Content = content
        };

        /// <summary>
        /// Fluent entry point to the message class which yields a <see cref="Message"/> instance with its content
        /// serialised into a string from the <see cref="content"/> parameter.
        /// </summary>
        /// <param name="content">The content to serialise into a string and associate with the message.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <typeparam name="T">The type of object to serialise into a string.</typeparam>
        /// <returns>An asynchronous task yielding the configured <see cref="Message"/> instance.</returns>
        public static async Task<Message> WithContent<T>(T content, CancellationToken cancellationToken = default)
        {
            var serialisedContent = await content.Serialise(cancellationToken).ConfigureAwait(false);
            return WithContent(serialisedContent);
        }
    }
}
