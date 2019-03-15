using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Common.Standard.Extensions;

namespace Common.Standard.Toggles
{
    /// <summary>
    /// Toggles provider for implementing toggling in .Net applications
    /// </summary>
    /// <remarks>
    /// See: https://surfingthecode.com/feature-toggles-in-.net-tips-and-tricks/
    /// See Martin Fowler article at:  https://martinfowler.com/articles/feature-toggles.html 
    /// </remarks>
    public class ToggleProvider : IToggleProvider
    {
        #region Fields
        private static Lazy<ToggleProvider> _instance = new Lazy<ToggleProvider>();
        private bool _isInited = false;

        private ConcurrentDictionary<string, IToggle> _toggleDictionary;

        #endregion

        #region Properties
        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static ToggleProvider Instance
        {
            get { return _instance.Value; }
        }

        private ConcurrentDictionary<string, IToggle> ToggleDictionary
        {
            get
            {
                _toggleDictionary = _toggleDictionary ?? new ConcurrentDictionary<string, IToggle>();
                return _toggleDictionary;
            }
        }
        #endregion

        #region Ctors
        /// <summary>
        /// Default Ctor
        /// </summary>
        public ToggleProvider()
        {
                
        }
        #endregion

        #region Publics
        /// <summary>
        /// Initialize the toggles from settings using a Loader class
        /// </summary>
        /// <param name="loader">the loader class</param>
        public void Init(ITogglesLoader loader)
        {
            if (!_isInited)
            {
                if (loader != null)
                {
                    if (loader.LoadFromSource())
                    {
                        foreach (var toggle in loader.TogglesFound)
                        {
                            AddToggle(toggle);
                        }
                    }
                }

                _isInited = true;
            }
        }

        /// <summary>
        /// For Ad-hoc toggle adding to the provider
        /// </summary>
        /// <param name="key">the toggle key</param>
        /// <param name="startAt">When this toggle will become active</param>
        /// <returns>true is successfully added</returns>
        public bool AddToggle(string key, DateTime startAt)
        {
            key.ThrowIfNull();

            Toggle toggle = new Toggle()
            {
                Key = key,
                IsEnabled = true,
                Start = startAt
            };
            return AddToggle(toggle);
        }

        /// <summary>
        /// For Ad-hoc toggle adding to the provider
        /// </summary>
        /// <param name="toggle">the toggle to add</param>
        /// <returns>true is successfully added</returns>
        public bool AddToggle(IToggle toggle)
        {
            toggle.ThrowIfNull();
            if (!ToggleDictionary.ContainsKey(toggle.Key))
            {
                return ToggleDictionary.TryAdd(toggle.Key, toggle);
            }

            return false;
        }

        /// <summary>
        /// Set Toggle as enable or not
        /// </summary>
        /// <param name="key">the toggle key</param>
        /// <param name="isEnabled">true to enable false to disable</param>
        /// <returns>this ToggleProvider (Fluent)</returns>
        public ToggleProvider SetEnabled(string key, bool isEnabled)
        {
            key.ThrowIfNull();

            if (ToggleDictionary.ContainsKey(key))
            {
                ToggleDictionary[key].IsEnabled = isEnabled;
            }

            return this;
        }

        /// <summary>
        /// Sets the starting DateTime for this toggle
        /// </summary>
        /// <param name="key">the key for the toggle</param>
        /// <param name="startAt">the DateTime when the toggle is enabled</param>
        /// <returns>this ToggleProvider (Fluent)</returns>
        public ToggleProvider SetStart(string key, DateTime startAt)
        {
            if (ToggleDictionary.ContainsKey(key))
            {
                ToggleDictionary[key].Start = startAt;
            }

            return this;
        }

        /// <summary>
        /// Checks a Toggle
        /// </summary>
        /// <param name="key">the toggle key to find</param>
        /// <returns>True if the Enabled and the Start datetime has been reached or exceeded</returns>
        public bool IsToggled(string key)
        {
            if (ToggleDictionary.ContainsKey(key))
            {
                return ToggleDictionary[key].IsEnabled && ToggleDictionary[key].Start < DateTime.Now;
            }

            return false;
        }
        #endregion
    }
}
