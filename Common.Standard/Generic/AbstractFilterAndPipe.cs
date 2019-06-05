using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Contract for Pipes and Filters design pattern
    /// </summary>
    /// <typeparam name="T">the type for pattern execution</typeparam>
    public interface IFilterAndPipe<T>: IComparable<IFilterAndPipe<T>>, IComparer<IFilterAndPipe<T>>, IDisposable
    {
        /// <summary>
        /// The logical name of this Filter
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The ordinal sort order
        /// </summary>
        int SortOrder { get; }

        /// <summary>
        /// Use Parallel execution (no waiting for the filter action to complete)
        /// </summary>
        bool ParallelExecution { get; set; }

        /// <summary>
        /// Return true to continue piping to this filter, false halts this filter from piping
        /// </summary>
        /// <param name="item">the item to filter and pipe</param>
        /// <returns>true or false</returns>
        bool CanFilterItem(T item);

        /// <summary>
        /// Pipe to the next filter(s)
        /// </summary>
        /// <param name="item">the item to filter and pipe</param>
        void Pipe(T item);

        /// <summary>
        /// The filter action to take on each item
        /// </summary>
        /// <param name="item">the item to filter</param>
        void Filter(T item);

        /// <summary>
        /// Add a sub-filter to this filter
        /// </summary>
        /// <param name="nextFilter">the next filter to add</param>
        void AddFilter(IFilterAndPipe<T> nextFilter);

        /// <summary>
        /// Event handling for when and exception is encountered
        /// </summary>
        event EventHandler FilterExceptionEncountered;

        /// <summary>
        /// The filter has completed filtering and piping
        /// </summary>
        event EventHandler FilterCompleted;
    }

    /// <summary>
    /// Abstraction class of a Filter and Pipe 
    /// </summary>
    /// <typeparam name="T">the type to filter and pipe</typeparam>
    public abstract class AbstractFilterAndPipe<T> : IFilterAndPipe<T>
    {
        #region Fields
        private string _name;
        private int _sortOrder;
        private bool _parallelExecution;
        private List<IFilterAndPipe<T>> _filterList;
        #endregion

        #region Properties
        /// <summary>
        /// The logical name of this Filter
        /// </summary>
        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        /// <summary>
        /// The ordinal sort order
        /// </summary>
        public int SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        /// <summary>
        /// Use Parallel execution (no waiting for the filter action to complete)
        /// </summary>
        public bool ParallelExecution
        {
            get { return _parallelExecution; }
            set { _parallelExecution = value; }
        }

        private List<IFilterAndPipe<T>> FilterList
        {
            get
            {
                if (_filterList == null)
                {
                    _filterList = new List<IFilterAndPipe<T>>();
                }

                return _filterList;
            }
        }
        #endregion

        #region Events and delegates
        /// <summary>
        /// Event handling for when and exception is encountered
        /// </summary>
        public event EventHandler FilterExceptionEncountered;

        /// <summary>
        /// The filter has completed filtering and piping
        /// </summary>
        public event EventHandler FilterCompleted;
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="name">Logical name of Filter</param>
        /// <param name="parallelExecution">use parallel execution (default = false)</param>
        public AbstractFilterAndPipe(string name, bool parallelExecution = false)
        {
            Name = name;
            ParallelExecution = parallelExecution;
        }
        #endregion

        #region Abstracts
        /// <summary>
        /// Return true to continue piping to this filter, false halts this filter from piping
        /// </summary>
        /// <param name="item">the item to filter and pipe</param>
        /// <returns>true or false</returns>
        public abstract bool CanFilterItem(T item);

        /// <summary>
        /// The filter action to take on each item
        /// </summary>
        /// <param name="item">the item to filter</param>
        public abstract void Filter(T item);
        #endregion

        #region Publics
        /// <summary>
        /// Add a sub-filter to this filter
        /// </summary>
        /// <param name="nextFilter">the next filter to add</param>
        public void AddFilter(IFilterAndPipe<T> nextFilter)
        {
            nextFilter.FilterCompleted += NextFilter_FilterCompleted;
            nextFilter.FilterExceptionEncountered += NextFilter_FilterExceptionEncountered;

            FilterList.Add(nextFilter);
            FilterList.Sort();
        }

        /// <summary>
        /// Pipe to the next filter(s)
        /// </summary>
        /// <param name="item">the item to filter and pipe</param>
        public void Pipe(T item)
        {
            if (ParallelExecution)
            {
                foreach (var filterAndPipe in FilterList)
                {
                    ExecutePipe(filterAndPipe, item);
                }

                ExecuteFilter(item);
            }
            else
            {
                ExecuteFilter(item);

                foreach (var filterAndPipe in FilterList)
                {
                    ExecutePipe(filterAndPipe, item);
                }
            }

            FilterCompleteEventArgs eventArgs = new FilterCompleteEventArgs(Name);
            FilterCompleted?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// The difference between this.SortOrder and the other.SortOrder
        /// </summary>
        /// <param name="other">the other filter</param>
        /// <returns>The difference between this.SortOrder and the other.SortOrder</returns>
        public int CompareTo(IFilterAndPipe<T> other)
        {
            return SortOrder - other.SortOrder;
        }

        /// <summary>
        /// Compare two filters and return difference is SortOrder
        /// </summary>
        /// <param name="x">filter</param>
        /// <param name="y">another filter</param>
        /// <returns></returns>
        public int Compare(IFilterAndPipe<T> x, IFilterAndPipe<T> y)
        {
            return x.CompareTo(y);
        }
        #endregion

        #region Protected virtuals
        /// <summary>
        /// Override this method to add disposal of managed objects
        /// </summary>
        protected virtual void AdditionalManagedDispose()
        {

        }

        /// <summary>
        /// Override this method to add disposal of unmanaged objects
        /// </summary>
        protected virtual void AdditionalUnManagedDispose()
        {

        }
        #endregion

        #region Privates

        private void ExecuteFilter(T item)
        {
            try
            {
                Filter(item);
                FilterCompleteEventArgs eventArgs = new FilterCompleteEventArgs(Name);
                eventArgs.Message += $"Complete";

                FilterCompleted?.Invoke(this, eventArgs);
            }
            catch (Exception e)
            {
                FilterExceptionEventArgs eventArgs = new FilterExceptionEventArgs(Name){Exception = e};

                FilterExceptionEncountered?.Invoke(this, eventArgs);
            }
        }

        private void ExecutePipe(IFilterAndPipe<T> filter, T item)
        {
            try
            {
                if(filter.CanFilterItem(item))
                    filter.Pipe(item);
            }
            catch (Exception e)
            {
                FilterExceptionEventArgs eventArgs = new FilterExceptionEventArgs(Name) { Exception = e };
                FilterExceptionEncountered?.Invoke(this, eventArgs);
            }
        }
        #endregion

        #region Event handling
        private void NextFilter_FilterExceptionEncountered(object sender, EventArgs e)
        {
            FilterExceptionEncountered?.Invoke(this, e);
        }

        private void NextFilter_FilterCompleted(object sender, EventArgs e)
        {
            FilterCompleted?.Invoke(this, e);
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                foreach (var filterAndPipe in FilterList)
                {
                    filterAndPipe.Dispose();
                }

                if (disposing)
                {
                    AdditionalManagedDispose();
                }

                AdditionalUnManagedDispose();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AbstractFilterAndPipe() {
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
