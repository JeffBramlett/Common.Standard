using System;

namespace Common.Standard.Toggles
{
    /// <summary>
    /// Implementation of a ToggleProvider
    /// </summary>
    public interface IToggleProvider
    {
        /// <summary>
        /// Initialize the ToggleProvider
        /// </summary>
        /// <param name="loader">the loader class to supply the toggles from setting or db and etc.</param>
        void Init(ITogglesLoader loader);

        /// <summary>
        /// Adds a Toggle to the provider
        /// </summary>
        /// <param name="toggle">the toggle to add</param>
        /// <returns>true if successful</returns>
        bool AddToggle(IToggle toggle);

        /// <summary>
        /// Add a Toggle to the provider
        /// </summary>
        /// <param name="key">the key to the Toggle</param>
        /// <param name="startAt">the datetime the Toggle becomes active</param>
        /// <returns>true if successful</returns>
        bool AddToggle(string key, DateTime startAt);

        /// <summary>
        /// Check the Toggle
        /// </summary>
        /// <param name="key">the key to the toggle</param>
        /// <returns>True if togged and false otherwise</returns>
        bool IsToggled(string key);

        /// <summary>
        /// Set a Toggle to be enabled/disabled
        /// </summary>
        /// <param name="key">the key to the Toggle</param>
        /// <param name="isEnabled">true to enable and false to disable</param>
        /// <returns></returns>
        ToggleProvider SetEnabled(string key, bool isEnabled);

        /// <summary>
        /// Set the starting DateTime for when the Toggle becomes active
        /// </summary>
        /// <param name="key">the key to the toggle</param>
        /// <param name="startAt">the datetime to start the Toggle</param>
        /// <returns></returns>
        ToggleProvider SetStart(string key, DateTime startAt);
    }
}