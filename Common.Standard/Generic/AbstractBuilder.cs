using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Abstract of a Builder for objects of type T
    /// </summary>
    /// <typeparam name="T">the type of object to build</typeparam>
    public abstract class AbstractBuilder<T> : IGenericBuilder<T> where T : class, new()
    {
        #region fields
        private bool disposedValue;
        private List<DelegateInfo<T>> _delegates;
        private T _currentItem;
        #endregion

        #region Properties
        List<DelegateInfo<T>> Delegates
        {
            get
            {
                _delegates = _delegates ?? new List<DelegateInfo<T>>();
                return _delegates;
            }
        }

        private T CurrentItem
        {
            get 
            { 
                _currentItem = _currentItem ?? new T();
                return _currentItem;
            }
            set { _currentItem = value; }
        }
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        public AbstractBuilder()
        {

        }

        ~AbstractBuilder()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Build the object of type T using the build delegates in the sorted order
        /// </summary>
        /// <param name="args">the arguments to use when building the object</param>
        /// <returns>a new object of type T</returns>
        public T Build(params object[] args)
        {
            Delegates.Sort();

            foreach(var del in Delegates)
            {
                var success = del.Delegate(CurrentItem, args);

                if(!success)
                {
                    throw new ArgumentException();
                }
            }

            var toReturn = CurrentItem;
            CurrentItem = null;
            return toReturn;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Add a delegate for the build process
        /// </summary>
        /// <param name="action">the delegate to use in the building process</param>
        /// <param name="ordinal">the sort order of the delegate</param>
        protected void AddDelegate(Func<T, object[], bool> action, int ordinal = int.MaxValue)
        {
            DelegateInfo<T> info = new DelegateInfo<T>()
            {
                Delegate = action,
                Ordinal = ordinal
            };

            Delegates.Add(info);
        }
        #endregion

        #region Disposing
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                ReleaseForDispose();

                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose the builder and release the resources
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void ReleaseForDispose()
        {

        }
        #endregion
    }

    #region Inner Classes
    class DelegateInfo<T>: IComparable<DelegateInfo<T>>, IComparer<DelegateInfo<T>>
    {
        public int Ordinal { get; set; }
        public Func<T,object[], bool> Delegate { get; set; }

        public int Compare(DelegateInfo<T> x, DelegateInfo<T> y)
        {
            return x.Ordinal - y.Ordinal;
        }

        public int CompareTo(DelegateInfo<T> other)
        {
            return Ordinal - other.Ordinal;
        }
    }
    #endregion

}
