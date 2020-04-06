using System;

namespace Moogie.Queues.Providers.Memory
{
    internal class MessageRepresentation
    {
        public Guid Id { get; set; }

        public string Message { get; set; }

        public DateTime? Expiry { get; set; }
    }
}
