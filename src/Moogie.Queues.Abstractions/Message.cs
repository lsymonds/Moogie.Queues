namespace Moogie.Queues
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
    }
}
