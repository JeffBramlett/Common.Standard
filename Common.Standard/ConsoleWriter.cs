using System;
using System.Threading.Tasks;
using Common.Standard.Generic;

namespace Common.Standard
{
    public class ConsoleWriter: IDisposable
    {
        #region Enums (private)
        private enum WriteModes
        {
            Information,
            Emphasis,
            Important,
            Warning
        }
        #endregion

        #region Fields
        
        private GenericSpooler<WriteInfo> _writeSpooler;
        #endregion

        #region Properties
        private GenericSpooler<WriteInfo> WriteSpooler
        {
            get
            {
                if (_writeSpooler == null)
                {
                    _writeSpooler = new GenericSpooler<WriteInfo>();
                    _writeSpooler.ItemSpooled += _writeSpooler_ItemSpooled;
                }

                return _writeSpooler;
            }
        }

        private void _writeSpooler_ItemSpooled(WriteInfo item)
        {
            var previousForeColor = Console.ForegroundColor;
            var previousBackgroundColor = Console.BackgroundColor;

            switch (item.WriteMode)
            {
                case WriteModes.Information:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(item.MessageToWrite);
                    break;
                case WriteModes.Emphasis:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(item.MessageToWrite);
                    break;
                case WriteModes.Important:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(item.MessageToWrite);
                    break;
                case WriteModes.Warning:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(item.MessageToWrite);
                    break;
            }


            Console.Write("\b \b");

            Console.ForegroundColor = previousForeColor;
            Console.BackgroundColor = previousBackgroundColor;
        }
        #endregion

        #region Singleton
        private static Lazy<ConsoleWriter> _staticWriter = new Lazy<ConsoleWriter>();
        public static ConsoleWriter Instance
        {
            get { return _staticWriter.Value; }
        }
        #endregion

        #region Ctors and Dtors

        public ConsoleWriter()
        {
                
        }
        #endregion

        #region Public Write To Methods

        public void WriteInformation(string message, bool waitForKey = false, ConsoleKey keyToWaitFor = ConsoleKey.Escape)
        {
            WriteSpooler.AddItem(new WriteInfo(WriteModes.Information, message));
            if (waitForKey)
            {
                while (true)
                {
                    var keyPressed = Console.ReadKey().Key;
                    Console.Write("\b \b");

                    if (keyToWaitFor == ConsoleKey.Escape || keyPressed == keyToWaitFor)
                        break;
                }
            }
        }

        public void WriteEmphasis(string message, bool waitForKey = false, ConsoleKey keyToWaitFor = ConsoleKey.Escape)
        {
            WriteSpooler.AddItem(new WriteInfo(WriteModes.Emphasis, message));
            if (waitForKey)
            {
                while (true)
                {
                    var keyPressed = Console.ReadKey().Key;
                    Console.Write("\b \b");

                    if (keyToWaitFor == ConsoleKey.Escape || keyPressed == keyToWaitFor)
                        break;
                }
            }
        }

        public void WriteImportant(string message, bool waitForKey = false, ConsoleKey keyToWaitFor = ConsoleKey.Escape)
        {
            WriteSpooler.AddItem(new WriteInfo(WriteModes.Important, message));
            if (waitForKey)
            {
                while (true)
                {
                    var keyPressed = Console.ReadKey().Key;
                    Console.Write("\b \b");

                    if (keyToWaitFor == ConsoleKey.Escape || keyPressed == keyToWaitFor)
                        break;
                }
            }
        }

        public void WriteWarning(string message, bool waitForKey = false, ConsoleKey keyToWaitFor = ConsoleKey.Escape)
        {
            WriteSpooler.AddItem(new WriteInfo(WriteModes.Warning, message));
            if (waitForKey)
            {
                while (true)
                {
                    var keyPressed = Console.ReadKey().Key;
                    Console.Write("\b \b");

                    if (keyToWaitFor == ConsoleKey.Escape || keyPressed == keyToWaitFor)
                        break;
                }
            }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_writeSpooler != null)
                    {
                        _writeSpooler.ItemSpooled -= _writeSpooler_ItemSpooled;
                        _writeSpooler.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        ~ConsoleWriter()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Inner classes
        /// <summary>
        /// Not publically accessible class for console writer
        /// </summary>
        private class WriteInfo
        {
            public WriteModes WriteMode { get; set; }

            public string MessageToWrite { get; set; }

            public WriteInfo(WriteModes writeMode, string message)
            {
                WriteMode = writeMode;
                MessageToWrite = message;
            }
        }
        #endregion
    }
}