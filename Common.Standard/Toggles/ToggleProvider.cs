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

        #endregion

        #region Properties

        private IToggleRepository ToggleRepository { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Default Ctor
        /// </summary>
        public ToggleProvider(IToggleRepository toggleRepository)
        {
            toggleRepository.ThrowIfNull(nameof(toggleRepository));
            ToggleRepository = toggleRepository;
        }
        #endregion

        #region Publics

        /// <summary>
        /// For Ad-hoc toggle adding to the provider
        /// </summary>
        /// <param name="key">the toggle key</param>
        /// <param name="startAt">When this toggle will become active</param>
        /// <returns>true is successfully added</returns>
        public void AddToggle(string key, DateTime startAt)
        {
            key.ThrowIfNull();

            IToggle toggle = new Toggle()
            {
                Key = key,
                IsEnabled = true,
                Start = startAt
            };
            AddToggle(toggle);
        }

        /// <summary>
        /// For Ad-hoc toggle adding to the provider
        /// </summary>
        /// <param name="toggle">the toggle to add</param>
        /// <returns>true is successfully added</returns>
        public void AddToggle(IToggle toggle)
        {
            toggle.ThrowIfNull();
            if (!ToggleRepository.HasToggleByKey(toggle.Key))
            {
                ToggleRepository.AddToggle(toggle);
            }
        }

        /// <summary>
        /// Checks a Toggle
        /// </summary>
        /// <param name="key">the toggle key to find</param>
        /// <returns>True if the Enabled and the Start datetime has been reached or exceeded</returns>
        public bool IsToggled(string key)
        {
            if (ToggleRepository.HasToggleByKey(key))
            {
                var toggle = ToggleRepository.ToggleForKey(key);
                return toggle.IsEnabled && toggle.Start < DateTime.Now;
            }

            return false;
        }
        #endregion
    }
}
