using System;

namespace Moogie.Queues
{
    /// <summary>
    /// An interface defining required properties for provider Dispatch methods.
    /// </summary>
    public interface IProviderDispatchable
    {
        /// <summary>
        /// Gets or sets the id of the dispatchable message.
        /// </summary>
        Guid? Id { get; }

        /// <summary>
        /// Gets or sets the content of the message.
        /// </summary>
        string? Content { get; }

        /// <summary>
        /// Gets or sets when the message expires.
        /// </summary>
        DateTime? Expiry { get; }
    }
}
