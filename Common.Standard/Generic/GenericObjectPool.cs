using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Common.Standard.Generic
{
    public enum BalancingMethods
    {
        RoundRobin,
        Random,
        MaxCount
    }
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
        Task<T> AcquireItem(params object[] activateObjects);

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
        private IList<T> _itemPool;
        private readonly int _poolSize = 10;

        private Random _loadingRandom;
        #endregion

        #region Properties

        private IList<T> ItemPool
        {
            get
            {
                return _itemPool;
            }
        }

        protected int PoolSize
        {
            get { return _poolSize; }
        }

        private int CurrentIndex { get; set; }

        private BalancingMethods BalancingMethod { get; set; }

        /// <summary>
        /// Return the count of active items
        /// </summary>
        public int Count
        {
            get
            {
                var stilActive = ItemPool.Count();
                return stilActive;
            }
        }

        /// <summary>
        /// Return the allocated size of the pool
        /// </summary>
        public int Size
        {
            get { return ItemPool.Count; }
        }

        #endregion

        #region Event and delegates

        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="poolSize">the size of the pool</param>
        /// <param name="balancingMethod">When acquiring item which one gets returned</param>
        public GenericObjectPool(int poolSize = 10, BalancingMethods balancingMethod = BalancingMethods.RoundRobin)
        {
            if(poolSize <= 0)
                throw new ArgumentOutOfRangeException("poolSize size must be greater than 0");

            BalancingMethod = balancingMethod;

            _poolSize = poolSize;

            Initialize().Wait();
        }

        /// <summary>
        /// Finalizer for Object Pool
        /// </summary>
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
        public async Task<T> AcquireItem(params object[] activateObjects)
        {
            T firstFound = await FindOrCreateLoader();

            firstFound.Activate(activateObjects);

            return firstFound;
        }

        private async Task<T> FindOrCreateLoader()
        {
            T firstFound;
            switch (BalancingMethod)
            {
                case BalancingMethods.Random:
                    if (_loadingRandom == null)
                    {
                        _loadingRandom = new Random();
                    }

                    CurrentIndex = _loadingRandom.Next(0, ItemPool.Count());
                    break;
                case BalancingMethods.MaxCount:
                    int maxedCount = 0;
                    for (var i = 0; i < ItemPool.Count(); i++)
                    {
                        var openItem = ItemPool.ElementAt(i);
                        if (openItem.ActiveCount >= openItem.MaxCount)
                            maxedCount++;
                        else
                        {
                            CurrentIndex = i;
                            break;
                        }
                    }
                    if (maxedCount == ItemPool.Count())
                    {
                        var newItem = new T();
                        ItemPool.Add(newItem);
                        CurrentIndex = ItemPool.Count() - 1;
                    }
                    break;
                default:
                    if (CurrentIndex >= ItemPool.Count() )
                        CurrentIndex = 0;
                    break;
            }


            firstFound = ItemPool.ElementAt(CurrentIndex);

            CurrentIndex++;

            return await Task.FromResult(firstFound);
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
            });
        }

        /// <summary>
        /// Contract the Pool to active size
        /// </summary>
        /// <returns></returns>
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

        private void ContractPool()
        {
            if (_itemPool == null)
                return;

            if(ItemPool.Count > PoolSize)
            {
                for(var i = PoolSize -1; i < ItemPool.Count; i++)
                {
                    ItemPool[i].Dispose();
                    ItemPool.RemoveAt(i);
                }
            }
        }

        private void InitPool()
        {
            _itemPool = new List<T>();

            for (var i = 0; i < PoolSize; i++)
            {
                _itemPool.Add( new T());
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
                for (var i = 0; i < ItemPool.Count; i++)
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