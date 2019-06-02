using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Contract:
    /// Performance can be sometimes the key issue during the software development and the
    /// object creation(class instantiation) is a costly step. The Object Pool pattern
    /// offers a mechanism to reuse objects that are expensive to create. 
    /// </summary>
    /// <typeparam name="T">the type of item for the Pool</typeparam>
    /// <remarks>
    /// Based on: https://www.oodesign.com/object-pool-pattern.html
    /// </remarks>
    public interface IGenericObjectPool<T>: IDisposable
    {
        /// <summary>
        /// Acquire an available item from the Pool
        /// </summary>
        /// <returns>the acquited item (or null if no more are available)</returns>
        Task<T> AcquireItem();

        /// <summary>
        /// Release the item back to the Pool
        /// </summary>
        /// <param name="item">the item to release</param>
        /// <returns>void</returns>
        Task ReleaseItem(T item);

        /// <summary>
        /// Event handler raised when all the available items in the pool are taken
        /// </summary>
        event EventHandler PoolHasNoAvailableItems;

        /// <summary>
        /// Event handler raised when an Item is acquired and activated
        /// </summary>
        event EventHandler ItemActivated;

        /// <summary>
        /// Event handler raised when and Item is released back to the Pool and deactivated
        /// </summary>
        event EventHandler ItemDeactivated;
    }

    /// <summary>
    /// Abstraction:
    /// Performance can be sometimes the key issue during the software development and the
    /// object creation(class instantiation) is a costly step. The Object Pool pattern
    /// offers a mechanism to reuse objects that are expensive to create. 
    /// </summary>
    /// <typeparam name="T">the type of item for the Pool</typeparam>
    /// <remarks>
    /// Based on: https://www.oodesign.com/object-pool-pattern.html
    /// </remarks>
    public class GenericObjectPool<T>: IGenericObjectPool<T> where T: class, IPoolItem, new()
    {
        #region Fields

        private bool _isInitialized = false;
        private T[] _itemPool;
        private readonly int _poolSize = 10;
        #endregion

        #region Properties

        private T[] ItemPool
        {
            get
            {
                return _itemPool;
            }
        }

        private int PoolSize
        {
            get { return _poolSize; }
        }
        #endregion

        #region Event and delegates

        /// <summary>
        /// Event handler raised when all the available items in the pool are taken
        /// </summary>
        public event EventHandler PoolHasNoAvailableItems;

        /// <summary>
        /// Event handler raised when an Item is acquired and activated
        /// </summary>
        public event EventHandler ItemActivated;

        /// <summary>
        /// Event handler raised when and Item is released back to the Pool and deactivated
        /// </summary>
        public event EventHandler ItemDeactivated;
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="poolSize">the size of the pool</param>
        public GenericObjectPool(int poolSize = 10)
        {
            if(poolSize <= 0)
                throw new ArgumentOutOfRangeException("poolSize size must be greater than 0");

            _poolSize = poolSize;

            Initialize().Wait();
        }

        ~GenericObjectPool()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Acquire an available item from the Pool
        /// </summary>
        /// <returns>the acquited item (or null if no more are available)</returns>
        public async Task<T> AcquireItem()
        {
            T firstFound = null;

            await Task.Run(() =>
            {
                var openItems = ItemPool.Where(i => !i.IsActive);
                if (openItems != null && openItems.Count() > 0)
                {
                    firstFound = openItems.ElementAt(0);
                    firstFound.Activate();
                    RaiseItemActivated(firstFound);
                }
                else
                {
                    PoolHasNoAvailableItemsEventArgs args = new PoolHasNoAvailableItemsEventArgs();
                    PoolHasNoAvailableItems?.Invoke(this, args);
                }
            });

            return firstFound;
        }

        /// <summary>
        /// Release the item back to the Pool
        /// </summary>
        /// <param name="item">the item to release</param>
        /// <returns>void</returns>
        public async Task ReleaseItem(T item)
        {
            await Task.Run(() =>
            {
                var found = ItemPool.First(i => i.Equals(item));
                found?.Deactivate();
                RaiseItemDeactivated(item);
            });
        }
        #endregion

        #region Protected
        /// <summary>
        /// Virtual method to dispose manageded items in extending classes
        /// </summary>
        protected virtual void AdditionalManagedDispose()
        {

        }

        /// <summary>
        /// Virtual method to dispose unmanaged items in extending classes
        /// </summary>
        protected virtual void AdditionalUnmanagedDispose()
        {

        }

        #endregion

        #region Privates

        private async Task Initialize()
        {
            if (!_isInitialized)
            {
                await Task.Run(() =>
                {
                    if (_itemPool == null)
                    {
                        _itemPool = new T[PoolSize];
                        for (var i = 0; i < PoolSize; i++)
                        {
                            _itemPool[i] = new T();
                        }
                    }
                    _isInitialized = true;
                });
            }
        }
        #endregion

        #region Event Handlers

        private void RaiseItemActivated(T item)
        {
            PoolItemActionEventArgs<T> paction = new PoolItemActionEventArgs<T>(item, PoolItemActionEventArgs<T>.PoolItemActions.Activated);
            ItemActivated?.Invoke(this, paction);
        }

        private void RaiseItemDeactivated(T item)
        {
            PoolItemActionEventArgs<T> paction = new PoolItemActionEventArgs<T>(item, PoolItemActionEventArgs<T>.PoolItemActions.Deactivated);
            ItemDeactivated?.Invoke(this, paction);
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                for (var i = 0; i < ItemPool.Length; i++)
                {
                    ItemPool[i].Deactivate();
                    ItemPool[i].Dispose();
                }

                if (disposing)
                {
                    AdditionalManagedDispose();
                }

                AdditionalUnmanagedDispose();

                disposedValue = true;
            }
        }

        /// <summary>
        /// Release any resources used by this object
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}