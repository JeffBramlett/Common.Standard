﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        /// Contract the Pool to the original size or the size of active items
        /// </summary>
        /// <returns>void</returns>
        Task ContractItemPool();

        /// <summary>
        /// Return the count of active items
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Return the allocated size of the pool
        /// </summary>
        int Size { get; }
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

        /// <summary>
        /// Return the count of active items
        /// </summary>
        public int Count
        {
            get
            {
                var stilActive = ItemPool.Where(i => i.IsActive).Count();
                return stilActive;
            }
        }

        /// <summary>
        /// Return the allocated size of the pool
        /// </summary>
        public int Size
        {
            get { return ItemPool.Length; }
        }

        #endregion

        #region Event and delegates

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
                }
                else
                {
                    var ndx = ExpandPool();
                    firstFound = _itemPool[ndx];
                }

                firstFound.Activate();
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
                if (found.IsActive)
                {
                    found?.Deactivate();
                }
            });
        }

        public async Task ContractItemPool()
        {
            await Task.Run(() =>
            {
                ContractPool();
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

        private void ItemIsDeactivated(object itemAsObject, EventArgs args)
        {
            if (itemAsObject is IPoolItem)
            {
                var item = (T) itemAsObject;
                item.Deactivate();
            }
        }

        private async Task Initialize()
        {
            if (!_isInitialized)
            {
                await Task.Run(() =>
                {
                    InitPool();

                    _isInitialized = true;
                });
            }
        }

        private int ExpandPool()
        {
            int ndx = _itemPool.Length;

            List<T> itemList = new List<T>(_itemPool);


            for (var i = 0; i < PoolSize; i++)
            {
                var newItem = new T();

                itemList.Add(newItem);
            }

            _itemPool = itemList.ToArray();

            return ndx;
        }


        private void ContractPool()
        {
            if (_itemPool == null)
                return;

            var stillActiveItems = _itemPool.Where(i => i.IsActive);

            List<T> remainingList = new List<T>(stillActiveItems);
            foreach (var stillActiveItem in stillActiveItems)
            {
                remainingList.Add(stillActiveItem);
            }

            if (remainingList.Count <= PoolSize)
            {
                _itemPool = null;

                InitPool();

                for (var i = 0; i < remainingList.Count; i++)
                {
                    _itemPool[i] = remainingList[i];
                }
            }
            else
            {
                _itemPool = remainingList.ToArray();
            }
        }

        private void InitPool()
        {
            _itemPool = new T[PoolSize];

            for (var i = 0; i < PoolSize; i++)
            {
                _itemPool[i] = new T();
            }
        }
        #endregion

        #region Event Handlers

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