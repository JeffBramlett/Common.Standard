using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Toggles
{
    /// <summary>
    /// Implementation of a Toggle loader
    /// </summary>
    public interface ITogglesLoader
    {
        /// <summary>
        /// Execute loading the Toggles from the source
        /// </summary>
        /// <returns>True if successful, false otherwise</returns>
        bool LoadFromSource();

        /// <summary>
        /// Enumeration of the Toggle found in LoadFromSource
        /// </summary>
        IEnumerable<Toggle> TogglesFound { get; }
    }
}
