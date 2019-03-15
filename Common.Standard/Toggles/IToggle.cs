using System;

namespace Common.Standard.Toggles
{
    /// <summary>
    /// Implementation of a Toggle
    /// </summary>
    public interface IToggle
    {
        /// <summary>
        /// The toggle is enabled
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// The key to the toggle (unique)
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// When the toggle is to become active
        /// </summary>
        DateTime Start { get; set; }
    }
}