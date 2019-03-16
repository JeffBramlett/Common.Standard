using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Toggles
{
    /// <summary>
    /// Implementation of a ToggleRepository
    /// </summary>
    public interface IToggleRepository: IDisposable
    {
        /// <summary>
        /// Initialize the Repository
        /// </summary>
        void Init();

        /// <summary>
        /// Does the Toggle exist
        /// </summary>
        /// <param name="key">the key of the Toggle</param>
        /// <returns>true if found, false otherwise</returns>
        bool HasToggleByKey(string key);

        /// <summary>
        /// Get the Toggle for the key
        /// </summary>
        /// <param name="key">the toggle key</param>
        /// <returns>the found Toggle or null if not found</returns>
        IToggle ToggleForKey(string key);

        /// <summary>
        /// Add a Toggle by key and starting date time
        /// </summary>
        /// <param name="key">the toggle key</param>
        /// <param name="startAt">the effective date time</param>
        void AddToggle(string key, DateTime startAt);

        /// <summary>
        /// Add a Toggle object
        /// </summary>
        /// <param name="toggle">the Toggle object</param>
        void AddToggle(IToggle toggle);

    }
}
