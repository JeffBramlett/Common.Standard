using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Standard.Generic
{
    public interface IFilterAndPipe<T>: IComparable<IFilterAndPipe<T>>, IComparer<IFilterAndPipe<T>>, IDisposable
    {
        string Name { get; }

        int SortOrder { get; }

        bool ParallelExecution { get; set; }

        bool CanFilterItem(T item);

        void Pipe(T item);

        void Filter(T item);

        void AddFilter(IFilterAndPipe<T> nextFilter);

        event EventHandler FilterExceptionEncountered;
        event EventHandler FilterCompleted;
    }

    public abstract class AbstractFilterAndPipe<T> : IFilterAndPipe<T>
    {
        #region Fields
        private string _name;
        private int _sortOrder;
        private bool _parallelExecution;
        private List<IFilterAndPipe<T>> _filterList;
        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        public int SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

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
        public event EventHandler FilterExceptionEncountered;
        public event EventHandler FilterCompleted;
        #endregion

        #region Ctors and Dtors

        public AbstractFilterAndPipe(string name, bool parallelExecution = false)
        {
            Name = name;
            ParallelExecution = parallelExecution;
        }
        #endregion

        #region Publics
        public void AddFilter(IFilterAndPipe<T> nextFilter)
        {
            nextFilter.FilterCompleted += NextFilter_FilterCompleted;
            nextFilter.FilterExceptionEncountered += NextFilter_FilterExceptionEncountered;

            FilterList.Add(nextFilter);
            FilterList.Sort();
        }

        private void NextFilter_FilterExceptionEncountered(object sender, EventArgs e)
        {
            FilterExceptionEncountered?.Invoke(this, e);
        }

        private void NextFilter_FilterCompleted(object sender, EventArgs e)
        {
            FilterCompleted?.Invoke(this, e);
        }

        public abstract bool CanFilterItem(T item);

        public abstract void Filter(T item);

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

        public int CompareTo(IFilterAndPipe<T> other)
        {
            return SortOrder - other.SortOrder;
        }

        public int Compare(IFilterAndPipe<T> x, IFilterAndPipe<T> y)
        {
            return x.CompareTo(y);
        }
        #endregion

        #region Protected virtuals

        protected virtual void AdditionalManagedDispose()
        {

        }
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
                filter.Pipe(item);
            }
            catch (Exception e)
            {
                FilterExceptionEventArgs eventArgs = new FilterExceptionEventArgs(Name) { Exception = e };
                FilterExceptionEncountered?.Invoke(this, eventArgs);
            }
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
