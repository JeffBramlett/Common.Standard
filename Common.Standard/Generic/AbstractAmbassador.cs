using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Contract:
    /// Create helper services that send network requests on behalf of a consumer service or application.
    /// An ambassador service can be thought of as an out-of-process proxy that is co-located with the
    /// client.
    /// </summary>
    /// <typeparam name="I">the type of the input</typeparam>
    /// <typeparam name="R">the type of the output</typeparam>
    /// <remarks>Based on: https://docs.microsoft.com/en-us/azure/architecture/patterns/ambassador</remarks>
    public interface IAmbassador<I, R> : IDisposable
    {
        /// <summary>
        /// Add object to the Ambassador work queue
        /// </summary>
        /// <param name="input"></param>
        void AddInputToQueue(I input);

        /// <summary>
        /// Event handling for Exception
        /// </summary>
        event EventHandler InputExceptionEncountered;

        /// <summary>
        /// Event handling for a completed object input
        /// </summary>
        event EventHandler InputCompleted;
    }

    /// <summary>
    /// Abstraction:
    /// Create helper services that send network requests on behalf of a consumer service or application.
    /// An ambassador service can be thought of as an out-of-process proxy that is co-located with the
    /// client.
    /// </summary>
    /// <typeparam name="I">the type of the input</typeparam>
    /// <typeparam name="R">the type of the output</typeparam>
    /// <remarks>Based on: https://docs.microsoft.com/en-us/azure/architecture/patterns/ambassador</remarks>
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
        /// <summary>
        /// Event handling for Exception
        /// </summary>
        public event EventHandler InputExceptionEncountered;

        /// <summary>
        /// Event handling for a completed object input
        /// </summary>
        public event EventHandler InputCompleted;
        #endregion

        #region Ctors and Dtors

        /// <summary>
        /// Finalizer
        /// </summary>
        ~AbstractAmbassador()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
        #endregion

        #region Public
        /// <summary>
        /// Add object to the Ambassador work queue
        /// </summary>
        /// <param name="input"></param>
        public void AddInputToQueue(I input)
        {
            InputSpooler.AddItem(input);
        }

        #endregion

        #region Protected

        /// <summary>
        /// The execution for each item that is input
        /// </summary>
        /// <param name="input">the item input</param>
        /// <returns></returns>
        protected abstract R ExecuteOnEachInput(I input);

        /// <summary>
        /// Virtual method to override for disposing managed items
        /// </summary>
        protected virtual void AdditionalManagedDispose()
        {

        }

        /// <summary>
        /// Virtual method to override for disposing unmanaged items
        /// </summary>
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

        private void Dispose(bool disposing)
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

        /// <summary>
        /// Event Args class for input exceptions
        /// </summary>
        public class InputExceptionEventArgs : EventArgs
        {
            /// <summary>
            /// The input for the Ambassador
            /// </summary>
            public I Input { get; set; }

            /// <summary>
            /// Exception encountered
            /// </summary>
            public Exception Exception { get; set; }

            /// <summary>
            /// Default Ctor
            /// </summary>
            /// <param name="input">the input</param>
            /// <param name="e">the exception encountered for the input</param>
            public InputExceptionEventArgs(I input, Exception e)
            {
                Input = input;
                Exception = e;
            }
        }

        /// <summary>
        /// Event args class for input completion
        /// </summary>
        public class InputCompletedEventArgs : EventArgs
        {
            /// <summary>
            /// The input complete
            /// </summary>
            public I Input { get; set; }

            /// <summary>
            /// The response for the input
            /// </summary>
            public R Response { get; set; }

            /// <summary>
            /// The default Ctor
            /// </summary>
            /// <param name="input">the input</param>
            /// <param name="response">the response</param>
            public InputCompletedEventArgs(I input, R response)
            {
                Input = input;
                Response = response;
            }
        }
        #endregion
    }
}
