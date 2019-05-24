using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Standard.Generic
{
    public interface IAmbassador<I, R> : IDisposable
    {
        void AddInputToQueue(I input);

        event EventHandler InputExceptionEncountered;
        event EventHandler InputCompleted;
    }

    public abstract class AbstractAmbassador<I, R> : IAmbassador<I, R>
    {
        #region Fields

        private GenericSpooler<I> _inputSpooler;
        #endregion

        #region Properties

        private GenericSpooler<I> InputSpooler
        {
            get
            {
                if (_inputSpooler == null)
                {
                    _inputSpooler = new GenericSpooler<I>();
                    _inputSpooler.ItemSpooled += InputSpooler_ItemSpooled;
                    _inputSpooler.ExceptionEncountered += InputSpooler_ExceptionEncountered;
                }

                return _inputSpooler;
            }
        }

        private void InputSpooler_ExceptionEncountered(object sender, Exception ex)
        {
            RaiseException(default(I), ex);
        }

        private void InputSpooler_ItemSpooled(I item)
        {
            try
            {
                var response = ExecuteOnEachInput(item);
                RaiseCompleted(item, response);
            }
            catch (Exception e)
            {
                RaiseException(item, e);
            }
        }
        #endregion

        #region Events and delegates
        public event EventHandler InputExceptionEncountered;
        public event EventHandler InputCompleted;
        #endregion

        #region Ctors and Dtors

        ~AbstractAmbassador()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
        #endregion

        #region Public
        public void AddInputToQueue(I input)
        {
            InputSpooler.AddItem(input);
        }

        #endregion

        #region Protected

        protected abstract R ExecuteOnEachInput(I input);

        protected virtual void AdditionalManagedDispose()
        {

        }
        protected virtual void AdditionalUnManagedDispose()
        {

        }
        #endregion

        #region Privates

        private void RaiseException(I input, Exception e)
        {
            Task.Run(() =>
            {
                InputExceptionEventArgs args = new InputExceptionEventArgs(input, e);
                InputExceptionEncountered?.Invoke(this, args);
            });
        }

        private void RaiseCompleted(I input, R response)
        {
            Task.Run(() =>
            {
                InputCompletedEventArgs args = new InputCompletedEventArgs(input, response);
                InputCompleted?.Invoke(this, args);
            });
        }
        #endregion


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (_inputSpooler != null)
                {
                    _inputSpooler.ItemSpooled -= InputSpooler_ItemSpooled;
                    _inputSpooler.ExceptionEncountered -= InputSpooler_ExceptionEncountered;

                    _inputSpooler.Dispose();
                }

                if (disposing)
                {
                    AdditionalManagedDispose();
                }

                AdditionalUnManagedDispose();

                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose of resources
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Public inner classes

        public class InputExceptionEventArgs : EventArgs
        {
            public I Input { get; set; }
            public Exception Exception { get; set; }

            public InputExceptionEventArgs(I input, Exception e)
            {
                Input = input;
                Exception = e;
            }
        }

        public class InputCompletedEventArgs : EventArgs
        {
            public I Input { get; set; }
            public R Response { get; set; }

            public InputCompletedEventArgs(I input, R response)
            {
                Input = input;
                Response = response;
            }
        }
        #endregion
    }
}
