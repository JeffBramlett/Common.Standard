using Common.Standard.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConsistentHashRing
{
    /// <summary>
    /// The HashRing implementation
    /// </summary>
    /// <typeparam name="T">the type of item to use this HashRing with</typeparam>
    public class HashRing<T>: IDisposable
    {
        #region Fields
        SortedDictionary<UInt32, IHashRingLocation<T>> _locations;
        private bool disposedValue;
        #endregion

        #region Properties
        private SortedDictionary<UInt32, IHashRingLocation<T>> LocationDictionary
        {
            get
            {
                if (_locations == null)
                {
                    _locations = new SortedDictionary<uint, IHashRingLocation<T>>();
                    HashRingLocation<T> maxLocation = new HashRingLocation<T>(uint.MaxValue, default(T));
                    maxLocation.ReceivedItem += Location_ReceivedItem;
                    _locations.Add(uint.MaxValue, maxLocation);
                }
                return _locations;
            }
        }

        /// <summary>
        /// The locations collection
        /// </summary>
        public List<IHashRingLocation<T>> Locations
        {
            get
            {
                return LocationDictionary.Values.ToList();
            }
        }
        #endregion

        #region Events and Delegates
        public delegate void RingLocationReceivedItemDelegate(UInt32 key, T itemReceived);
        public event RingLocationReceivedItemDelegate RingLocationReceived;
        #endregion

        #region Publics for Location
        /// <summary>
        /// Add a location to HashRing
        /// </summary>
        /// <param name="item">the item to add</param>
        public HashRingLocation<T> AddLocation(T item)
        {
            item.ThrowIfNull();

            UInt32 key = Hashing.HashItem(item);

            HashRingLocation<T> location = new HashRingLocation<T>(key, item);
            location.ReceivedItem += Location_ReceivedItem;

            LocationDictionary.Add(key, location);

            return location;
        }

        /// <summary>
        /// Add a HashRingLocation to the HashRing
        /// </summary>
        /// <param name="location">the HashRingLocation</param>
        public void AddLocation(IHashRingLocation<T> location)
        {
            location.ReceivedItem += Location_ReceivedItem;

            LocationDictionary.Add(location.Key, location);
        }

        /// <summary>
        /// is there a location for this item
        /// </summary>
        /// <param name="item">the item</param>
        /// <returns>true if there is a location, false otherwise</returns>
        public bool HasLocation(T item)
        {
            item.ThrowIfNull();

            UInt32 key = Hashing.HashItem(item);
            return LocationDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Remove the locations and remap the items it contains
        /// </summary>
        /// <param name="item">the item for the location</param>
        public void RemoveLocation(IHashRingLocation<T> location)
        {
            LocationDictionary.Remove(location.Key);
        }

        /// <summary>
        /// How many locations does this HashRing have
        /// </summary>
        public int LocationCount
        {
            get { return LocationDictionary.Count; }
        }
        #endregion

        #region Publics for Items
        /// <summary>
        /// Add an Item to the HashRing
        /// </summary>
        /// <param name="item">the item to add</param>
        public void AddItem(T item)
        {
            item.ThrowIfNull();

            bool isAdded = false;

            UInt32 key = Hashing.HashItem(item);

            foreach(var locationKey in LocationDictionary.Keys)
            {
                if(key < locationKey)
                {
                    LocationDictionary[locationKey].AddToRing(item);
                    isAdded = true;
                    break;
                }
            }
            if(!isAdded)
            {

            }
        }
        #endregion

        #region Privates
        private void Location_ReceivedItem(UInt32 key, T itemReceived)
        {
            RingLocationReceived?.Invoke(key, itemReceived);
        }
        #endregion

        #region Disposing
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_locations != null)
                    {
                        foreach(var location in _locations.Values)
                        {
                            location.ReceivedItem -= Location_ReceivedItem;
                        }
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~HashRing()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
