using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Standard.Generic
{
    /// <summary>
    /// An asynchronous generic executing class that controls spooling -- 
    /// putting jobs on a queue and taking them off one at a time. This 
    /// class will not block the adding of items in the spool and will 
    /// execute the event asynchronously.
    /// </summary>
    /// <typeparam name="T">The Type of object to provide spooling for</typeparam>
    public class GenericSpooler<T> : IGenericSpooler<T>
    {
        #region Class Scope Members
        bool _isDisposed;

        /// <summary>
        /// Threadsafe collection class
        /// </summary>
        readonly ConcurrentQueue<ItemMetaData> _inputs;

        Task _processWorkerTask;

        #endregion

        #region Properties
        /// <summary>
        /// True if there are Currently more items in the queue
        /// </summary>
        public bool HasMore
        {
            get
            {
                return _inputs.Count > 0;
            }
        }


        private CancellationTokenSource CancelToken
        {
            get; set;
        }
        #endregion

        #region Delegates and Events
        /// <summary>
        /// Item has been spooled to the event
        /// </summary>
        /// <param name="item">the item itself</param>
        public delegate void ItemSpooledDelegate(T item);

        /// <summary>
        /// Delegate for when the spooler is empty
        /// </summary>
        public delegate void SpoolerEmptyDelegate();

        /// <summary>
        /// Delegate to use when getting notification that an exception has occurred
        /// </summary>
        /// <param name="sender">could be either the spooler or the object containing the callback (the callback produced the exception)</param>
        /// <param name="ex">the exception caught</param>
        public delegate void ExceptionEncounteredDelegate(object sender, Exception ex);

        /// <summary>
        /// Get notification that an exception has occurred
        /// </summary>
        public event ExceptionEncounteredDelegate ExceptionEncountered;

        /// <summary>
        /// Event to notify the spooler has emptied
        /// </summary>
        public event SpoolerEmptyDelegate SpoolerEmpty;

        /// <summary>
        /// Item spooled event (this event raises (asynchronpously) for each item as it is spooled)
        /// </summary>
        public event ItemSpooledDelegate ItemSpooled;
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        public GenericSpooler()
        {
            _inputs = new ConcurrentQueue<ItemMetaData>();
            CancelToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Finalizer . . . 
        /// </summary>
        ~GenericSpooler()
        {
            Dispose(false);
        }

        #endregion

        #region Public
        /// <summary>
        /// Adds an Item of type T to the queue
        /// </summary>
        /// <param name="item">the item of type T</param>
        /// <param name="itemCausesStop">if true the spooler stops after this item and must be restarted</param>
        public void AddItem(T item, bool itemCausesStop = false)
        {
            try
            {
                ItemMetaData storedItem = new ItemMetaData()
                {
                    HoldOnItem = itemCausesStop,
                    Item = item
                };

                _inputs.Enqueue(storedItem);
                StartProcess();
            }
            catch (Exception ex)
            {
                RaiseException(this, ex);
            }
        }

        /// <summary>
        /// Stops spooling the items and then resumes
        /// </summary>
        public void Sort()
        {
            Stop();

            var itemsInList = new List<T>();

            ItemMetaData dequeuedItem;
            while (_inputs.TryDequeue(out dequeuedItem))
            {
                itemsInList.Add(dequeuedItem.Item);
            }

            itemsInList.Sort();

            Resume();
        }

        /// <summary>
        /// Stops the spooler and empties the Queue
        /// </summary>
        public void Reset()
        {
            try
            {
                ItemMetaData ignoredData;
                while (_inputs.TryDequeue(out ignoredData))
                {
                    // do nothing; just clear the queue
                }
            }
            catch (Exception ex)
            {
                RaiseException(this, ex);
            }
        }

        /// <summary>
        /// Stop spooling items.  Items can still be added/removed/replaced but will not be spooled out
        /// </summary>
        public void Stop()
        {
            try
            {
                CancelToken.Cancel();
            }
            catch (Exception ex)
            {
                RaiseException(this, ex);
            }
        }

        /// <summary>
        /// Starts the spooling again.  All items still in the queue are sent out.
        /// </summary>
        public void Resume()
        {
            try
            {
                CancelToken = new CancellationTokenSource();
                ProcessWhileHasInput(CancelToken.Token);
            }
            catch (Exception ex)
            {
                RaiseException(this, ex);
            }
        }
        #endregion

        #region Virtuals
        /// <summary>
        /// Virtual method to override for adding general disposal by extending classes
        /// </summary>
        public virtual void GeneralDispose()
        {

        }

        /// <summary>
        /// Virtual method to override for adding dispose called using the Dispose() by extending classes
        /// </summary>
        public virtual void DeterministicDispose()
        {

        }
        /// <summary>
        /// Virtual method to override for adding Finalizer disposal by calling the Destructor (finalize) on extending classes
        /// </summary>
        public virtual void FinalizeDispose()
        {

        }
        #endregion

        #region Privates
        /// <summary>
        /// Starts the thread that spools the items off the queue (if not yet started)
        /// </summary>
        private void StartProcess()
        {
            if(_processWorkerTask == null 
                || _processWorkerTask.IsCompleted 
                || _processWorkerTask.IsCanceled
                || _processWorkerTask.IsFaulted)
            {
                CancelToken = new CancellationTokenSource();
                _processWorkerTask = Task.Factory.StartNew(() => ProcessWhileHasInput(CancelToken.Token), CancelToken.Token);
            }
        }

        private void ProcessWhileHasInput(CancellationToken cancelToken)
        {
            try
            {
                if (cancelToken.IsCancellationRequested)
                    return;

                // Keep the thread alive unless the exit event is signaled
                ItemMetaData itemData;
                while (_inputs.TryDequeue(out itemData))
                {
                    if(itemData.HoldOnItem)
                    {
                        Stop();
                        return;
                    }
                    RaiseItemSpooledEvent(itemData.Item);
                }

                SpoolerEmpty?.Invoke();
            }
            catch (Exception ex)
            {
                // Normal call to abort for the thread, call in finalize will necessarily end here if 
                // the thread has not already been stopped
                RaiseException(this, ex);
            }
        }

        /// <summary>
        /// Call the event (if there are any listeners)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        private void RaiseException(object sender, Exception ex)
        {
            ExceptionEncountered?.Invoke(this, ex);
        }

        /// <summary>
        /// Raise the event for each item as it is spooled
        /// </summary>
        /// <param name="item">the item to spool</param>
        private void RaiseItemSpooledEvent(T item)
        {
            ItemSpooled?.Invoke(item);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Dispose implementation to type of disposing
        /// </summary>
        /// <param name="disposing">True for deterministic, false for finalization</param>
        private void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            // General cleanup logic here
            GeneralDispose();

            if (disposing) // Deterministic only cleanup
            {
                DeterministicDispose();
            }
            else // Finalizer only cleanup
            {
                FinalizeDispose();
            }

            // if the worker task is still running, abort it!
            if(CancelToken != null)
            {
                CancelToken.Cancel();
                CancelToken.Dispose();
            }

            _processWorkerTask = null;

            _isDisposed = true;
        }

        /// <summary>
        /// Release all resources (clears List)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Inner classes
        private class ItemMetaData
        {
            /// <summary>
            /// If true spooler stops after this item
            /// </summary>
            public bool HoldOnItem { get; set; }

            /// <summary>
            /// The item in the spool
            /// </summary>
            public T Item { get; set; }
        }
        #endregion
    }
}
