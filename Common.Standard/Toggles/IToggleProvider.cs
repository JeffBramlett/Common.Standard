using System;

namespace Common.Standard.Toggles
{
    /// <summary>
    /// Implementation of a ToggleProvider
    /// </summary>
    public interface IToggleProvider
    {
        /// <summary>
        /// Adds a Toggle to the provider
        /// </summary>
        /// <param name="toggle">the toggle to add</param>
        void AddToggle(IToggle toggle);

        /// <summary>
        /// Add a Toggle to the provider
        /// </summary>
        /// <param name="key">the key to the Toggle</param>
        /// <param name="startAt">the datetime the Toggle becomes active</param>
        void AddToggle(string key, DateTime startAt);

        /// <summary>
        /// Check the Toggle
        /// </summary>
        /// <param name="key">the key to the toggle</param>
        /// <returns>True if togged and false otherwise</returns>
        bool IsToggled(string key);
    }
}