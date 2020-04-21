using System;

namespace Moogie.Queues
{
    /// <summary>
    /// An exception that is thrown when a particular feature has not been implemented yet.
    /// </summary>
    public class FeatureNotYetSupportedException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FeatureNotYetSupportedException" /> class.
        /// </summary>
        /// <param name="feature">The feature that is not yet supported.</param>
        public FeatureNotYetSupportedException(string feature)
            : base($"The feature ({feature}) you are trying to use is not currently implemented. Feel free to give the respective issue from the Github project a thumbs up to show your interest in it being worked on or, if you're feeling extra nice, contribute a PR to add the feature to the library. Thank you!")
        {
        }
    }
}