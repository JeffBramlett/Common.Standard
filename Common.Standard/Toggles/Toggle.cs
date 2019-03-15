using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Toggles
{
    /// <summary>
    /// Data class for a Toggle
    /// </summary>
    public class Toggle : IToggle
    {
        /// <summary>
        /// The unique key for the Toggle
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Is it enabled
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Is it Active
        /// </summary>
        public DateTime Start { get; set; }
    }
}
