using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Moogie.Queues.Internal
{
    /// <summary>
    /// Wrapper for a number of <see cref="object"/> extensions.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Serialises an object into a JSON string.
        /// </summary>
        /// <param name="obj">The object to serialise.</param>
        /// <param name="cancellationToken">The optional cancellation token to use in asynchronous operations.</param>
        /// <returns>The serialised string.</returns>
        public static async Task<string> Serialise<T>(this T obj, CancellationToken cancellationToken = default)
        {
            using (var stream = new MemoryStream())
            {
                await JsonSerializer
                    .SerializeAsync(stream, obj, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                    return await streamReader.ReadToEndAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Attempts to deserialise the message into an instance of an object of type T.
        /// </summary>
        /// <param name="message">The message to deserialise.</param>
        /// <param name="cancellationToken">The optional cancellation token to use in asynchronous operations.</param>
        /// <typeparam name="T">The type to deserialise into.</typeparam>
        /// <returns>The instance of the deserialised object or null if the deserialisation failed.</returns>
        public static async Task<T> TryDeserialise<T>(this string message, CancellationToken cancellationToken = default)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(message)))
                return await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
        }
    }
}
