using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Execute commands (IAppCommand) asynchronously and in order
    /// </summary>
    public class AppCommandProcessor : IAppCommandProcessor
    {
        #region Fields
        private bool disposedValue;
        private GenericSpooler<IAppCommand> _commandSpooler;
        #endregion

        #region Properties
        /// <summary>
        /// Arguments to be passed to each Command
        /// </summary>
        public List<object> Arguments { get; set; }

        private GenericSpooler<IAppCommand> CommandSpooler
        {
            get
            {
                if(_commandSpooler == null)
                {
                    _commandSpooler = new GenericSpooler<IAppCommand>();
                    _commandSpooler.ExceptionEncountered += _commandSpooler_ExceptionEncountered;
                    _commandSpooler.ItemSpooled += _commandSpooler_ItemSpooled;
                }

                return _commandSpooler;
            }
        }

        private bool IsInitialized { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Default Ctor
        /// </summary>
        public AppCommandProcessor()
        {

        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~AppCommandProcessor()
        {
            Dispose(disposing: false);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Add a command to be processed
        /// </summary>
        /// <param name="command">the command to be processed</param>
        /// <returns>true if added, false is the add failed</returns>
        public async Task<bool> AddCommand(IAppCommand command)
        {
            if(!IsInitialized)
            {
                Init();

                IsInitialized = true;
            }

            await Task.Run(() =>
            {
                CommandSpooler.AddItem(command);
            });

            return true;
        }

        /// <summary>
        /// Initialize this instance of the AppCommandProcessor
        /// </summary>
        /// <param name="args">arguments to be passed to each command</param>
        public void Init(params object[] args)
        {
            Arguments = new List<object>();
            Arguments.AddRange(args);
        }
        #endregion

        #region Protected
        /// <summary>
        /// Virtual dispose addition that is called in the private dispose(bool) method
        /// </summary>
        protected virtual void AdditionalDispose()
        {

        }
        #endregion

        #region Private Event handling
        private void _commandSpooler_ItemSpooled(IAppCommand cmd)
        {
            ExecuteCommand(cmd).Wait();
        }

        private async Task ExecuteCommand(IAppCommand cmd)
        {
            if (await cmd.CanExecute(Arguments.ToArray()))
            {
                await cmd.Execute(Arguments.ToArray());
            }
        }

        private void _commandSpooler_ExceptionEncountered(object sender, Exception ex)
        {
            throw ex;
        }
        #endregion

        #region Finalizing and Disposing

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    AdditionalDispose();

                    if (_commandSpooler != null)
                    {
                        _commandSpooler.ExceptionEncountered -= _commandSpooler_ExceptionEncountered;
                        _commandSpooler.ItemSpooled -= _commandSpooler_ItemSpooled;

                        _commandSpooler.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
