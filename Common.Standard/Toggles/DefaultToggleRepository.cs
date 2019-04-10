using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common.Standard.Extensions;
using Newtonsoft.Json;

namespace Common.Standard.Toggles
{
    /// <summary>
    /// Executing class to provide access to toggles in Json
    /// </summary>
    public sealed class DefaultToggleRepository : IToggleRepository
    {
        #region Fields
        private ConcurrentDictionary<string, IToggle> _toggleDictionary;
        #endregion

        #region Properties
        private ConcurrentDictionary<string, IToggle> ToggleDictionary
        {
            get
            {
                _toggleDictionary = _toggleDictionary ?? new ConcurrentDictionary<string, IToggle>();
                return _toggleDictionary;
            }
        }
        #endregion

        #region Publics
        /// <summary>
        /// Initialize the Repository
        /// </summary>
        public void Init()
        {

        }

        /// <summary>
        /// Load toggles from a filename
        /// </summary>
        /// <param name="file">the filename</param>
        public void LoadFromJsonFile(string file)
        {
            file.ThrowIfNull();

            using (StreamReader sr = new StreamReader(file))
            {
                string content = sr.ReadToEnd();
                LoadFromJsonContent(content);
            }
        }

        /// <summary>
        /// Load toggles from JSON content
        /// </summary>
        /// <param name="jsonContent">JSON serialization of a Toggle array</param>
        public void LoadFromJsonContent(string jsonContent)
        {
            jsonContent.ThrowIfNull();
            Toggle[] deserialized = JsonConvert.DeserializeObject<Toggle[]>(jsonContent);
            foreach (var toggle in deserialized)
            {
                AddToggle(toggle);
            }
        }

        /// <summary>
        /// Add a Toggle (Ad-Hoc)
        /// </summary>
        /// <param name="key">Toggle Key</param>
        /// <param name="startAt">DateTime when this Toggle becomes active</param>
        public void AddToggle(string key, DateTime startAt)
        {
            if (!ToggleDictionary.ContainsKey(key))
            {
                Toggle toggle = new Toggle()
                {
                    Key = key,
                    IsEnabled = true,
                    Start = startAt
                };

                AddToggle(toggle);
            }
        }

        /// <summary>
        /// Add a Toggle (Ad-Hoc)
        /// </summary>
        /// <param name="toggle">the Toggle to add</param>
        public void AddToggle(IToggle toggle)
        {
            if (!ToggleDictionary.ContainsKey(toggle.Key))
            {
                ToggleDictionary.TryAdd(toggle.Key, toggle);
            }
        }

        /// <summary>
        /// Does the Toggle exist for the key
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>True if found, false otherwised</returns>
        public bool HasToggleByKey(string key)
        {
            key.ThrowIfNull();
            return ToggleDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Get the Toggle for the key
        /// </summary>
        /// <param name="key">the key for the Toggle</param>
        /// <returns>True if found and false otherwise</returns>
        public IToggle ToggleForKey(string key)
        {
            if (ToggleDictionary.ContainsKey(key))
            {
                IToggle toggle;
                if (ToggleDictionary.TryGetValue(key, out toggle))
                {
                    return toggle;
                }

                return null;
            }

            return null;
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DefaultToggleRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
