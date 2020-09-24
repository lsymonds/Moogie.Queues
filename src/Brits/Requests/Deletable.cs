namespace Brits
{
    /// <summary>
    /// Represents something which can be deleted. This class is inherited by individual provider deletable entities
    /// which contain the required attributes to delete a queue message.
    /// </summary>
    public abstract class Deletable
    {
        /// <summary>
        /// The queue to delete the message from.
        /// </summary>
        public string Queue { get; set; } = "default";
    }
}
