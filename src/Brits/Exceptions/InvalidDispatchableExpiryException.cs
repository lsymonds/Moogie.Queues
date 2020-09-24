using System;

namespace Brits
{
    /// <summary>
    /// Exception that is thrown when the Expiry property on the <see cref="Message"/> class is invalid.
    /// </summary>
    public class InvalidDispatchableExpiryException : Exception
    {
        /// <summary>
        /// Gets or sets the expiry value that was set against the <see cref="Message"/> instance.
        /// </summary>
        public DateTime? ExpiryValue { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="InvalidDispatchableExpiryException"/> class.
        /// </summary>
        /// <param name="expiryValue">
        /// The expiry value that was set against the <see cref="Message"/> instance.
        /// </param>
        public InvalidDispatchableExpiryException(DateTime? expiryValue)
            : base($"Invalid expiry date for Dispatchable. See {nameof(ExpiryValue)} property for more information.")
        {
            ExpiryValue = expiryValue;
        }
    }
}
