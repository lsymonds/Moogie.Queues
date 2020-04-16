using System.Collections.Generic;

namespace Moogie.Queues
{
    /// <summary>
    /// Represents something which can be deleted.
    /// </summary>
    public class Deletable
    {
        /// <summary>
        /// Gets or sets the receipt handle (which is the provider provided id of the message).
        /// </summary>
        public string ReceiptHandle { get; set; }

        /// <summary>
        /// Gets or sets the queue to delete the message from.
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// Creates a <see cref="Deletable"/> instance from a receipt handle. This is intended to be used with
        /// extension methods for a fluent interface.
        /// </summary>
        /// <param name="receiptHandle">The provider managed id of the message to delete.</param>
        /// <returns>The <see cref="Deletable"/> instance.</returns>
        public static Deletable WithReceiptHandle(string receiptHandle) => new Deletable
        {
            ReceiptHandle = receiptHandle
        };
    }
}
