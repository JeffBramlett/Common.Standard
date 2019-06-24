using System;
using System.Threading.Tasks;
using Common.Standard.Generic;

namespace Common.Standard
{
    /// <summary>
    /// Write to the console window for Information, Emphasis, Important, and Warning
    /// Useful for console applications (i.e. quick demos)
    /// </summary>
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

        /// <summary>
        /// Singleton of the ConsoleWriter
        /// </summary>
        public static ConsoleWriter Instance
        {
            get { return _staticWriter.Value; }
        }
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        public ConsoleWriter()
        {
                
        }


        /// <summary>
        /// Finalizer
        /// </summary>
        ~ConsoleWriter()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        #endregion

        #region Public Write To Methods
        /// <summary>
        /// Write message as Information
        /// </summary>
        /// <param name="message">the message to write to the console</param>
        /// <param name="waitForKey">true to wait for a key (optional, default = false)</param>
        /// <param name="keyToWaitFor">the key to wait for (optional, default = escape key)</param>
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

        /// <summary>
        /// Write message as Emphasis
        /// </summary>
        /// <param name="message">the message to write to the console</param>
        /// <param name="waitForKey">true to wait for a key (optional, default = false)</param>
        /// <param name="keyToWaitFor">the key to wait for (optional, default = escape key)</param>
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

        /// <summary>
        /// Write message as Important
        /// </summary>
        /// <param name="message">the message to write to the console</param>
        /// <param name="waitForKey">true to wait for a key (optional, default = false)</param>
        /// <param name="keyToWaitFor">the key to wait for (optional, default = escape key)</param>
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

        /// <summary>
        /// Write message as Warning
        /// </summary>
        /// <param name="message">the message to write to the console</param>
        /// <param name="waitForKey">true to wait for a key (optional, default = false)</param>
        /// <param name="keyToWaitFor">the key to wait for (optional, default = escape key)</param>
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

        /// <summary>
        /// Dispose fo the console writer
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
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

        /// <summary>
        /// Dispose of the ConsoleWriter
        /// </summary>
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