using System.IO;
using System.Text.Json;
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
        /// <returns>The serialised string.</returns>
        public static async Task<string> Serialise(this object obj)
        {
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(stream))
                    return await streamReader.ReadToEndAsync();
            }
        }
    }
}
