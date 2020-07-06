using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsistentHashRing
{
    /// <summary>
    /// Storage node for Items of type T
    /// </summary>
    /// <typeparam name="T">the type of item to store in the location</typeparam>
    public class HashRingLocation<T> : IHashRingLocation<T>
    {
        private bool disposedValue;
        #region Events and Delegates
        public delegate void ReceivedItemDelegate(UInt32 key, T itemReceived);
        public event ReceivedItemDelegate ReceivedItem;
        #endregion

        #region Properties
        /// <summary>
        /// Unique key for this Node
        /// </summary>
        public UInt32 Key { get; set; }

        /// <summary>
        /// The contained "payload"
        /// </summary>
        public T Item { get; set; }


        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor (must have key and item
        /// </summary>
        /// <param name="key">the key for the hash rign location</param>
        /// <param name="item">the item to define the key</param>
        public HashRingLocation(UInt32 key, T item)
        {
            Key = key;
            Item = item;
        }
        #endregion

        #region 
        /// <summary>
        /// Add the item (raises the ReceivedItem event from the location)
        /// </summary>
        /// <param name="item">the item to use</param>
        public void AddToRing(T item)
        {
            RaiseItemReceived(item).Wait();
        }
        #endregion

        #region Raise Event
        private async Task RaiseItemReceived(T item)
        {
            await Task.Run(() =>
            {
                ReceivedItem?.Invoke(Key, item);
            });
        }
        #endregion
    }
}
