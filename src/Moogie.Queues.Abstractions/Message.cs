namespace Moogie.Queues
{
    /// <summary>
    /// A representation of a message that can be dispatched on a queue.
    /// </summary>
    public class Message : QueueableMessage
    {
        /// <summary>
        /// Fluent entry point to the message class which yields a <see cref="Message"/> instance with the queue
        /// name configured.
        /// </summary>
        /// <param name="queue">The queue to dispatch on.</param>
        /// <returns>The configured <see cref="Message"/> instance.</returns>
        public static Message OnQueue(string queue) => new Message
        {
            Queue = queue
        };
    }
}
