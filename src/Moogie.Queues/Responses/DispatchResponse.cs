using System;

namespace Moogie.Queues
{
    /// <summary>
    /// The response object returned from the Dispatch method on queue providers and managers.
    /// </summary>
    public class DispatchResponse
    {
        /// <summary>
        /// Gets or sets the created client managed message id.
        /// </summary>
        public Guid MessageId { get; set; }
    }
}
