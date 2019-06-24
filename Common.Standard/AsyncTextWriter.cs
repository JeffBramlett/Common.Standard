using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Common.Standard.Extensions;
using Common.Standard.Generic;

namespace Common.Standard
{
    /// <summary>
    /// AsyncTextWriter implementation
    /// </summary>
    public interface IAsyncTextWriter : IDisposable
    {
        /// <summary>
        /// Initialize the writer to a filename
        /// </summary>
        /// <param name="filename">the name of the file to contain the contents</param>
        void Init(string filename);

        /// <summary>
        /// Write content to the file without a CR/LF
        /// </summary>
        /// <param name="content">the content to write</param>
        /// <param name="formatArgs">args for string.format</param>
        void Write(string content, params object[] formatArgs);

        /// <summary>
        /// Write a line of text to the file
        /// </summary>
        /// <param name="contentLine">the line of text</param>
        /// <param name="formatArgs">args for string.format</param>
        void WriteLine(string contentLine, params object[] formatArgs);
    }

    /// <summary>
    /// Executing class to write to text file asynchonously.
    /// Use the singleton (Instance property) or instantiate and use with Dependency Injection
    /// </summary>
    public class AsyncTextWriter : IAsyncTextWriter
    {
        #region Fields
        private static Lazy<AsyncTextWriter> _instance = new Lazy<AsyncTextWriter>();
        private GenericSpooler<WriteCommand> _spooler;
        private string _filename;
        private readonly object lockObj = new object();
        #endregion

        #region Properties

        private GenericSpooler<WriteCommand> Spooler
        {
            get
            {
                if (_spooler == null)
                {
                    _spooler = new GenericSpooler<WriteCommand>();
                    _spooler.ItemSpooled += WriteContent;
                }
                return _spooler;
            }
        }

        private string Filename
        {
            get
            {
                if (string.IsNullOrEmpty(_filename))
                {
                    DateTime now = DateTime.Now;
                    _filename = string.Format("{0} {1}-{1}-{2}.txt", Assembly.GetExecutingAssembly().GetName().Name, now.Day, now.Month, now.Year);
                }

                return _filename;
            }
        }
        #endregion

        #region Singleton

        /// <summary>
        /// Singleton (thread-safe) instance of the writer
        /// </summary>
        public static IAsyncTextWriter Instance
        {
            get { return _instance.Value; }
        }
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Required parameterless Ctor for lazy instance
        /// </summary>
        public AsyncTextWriter()
        {
        }

        /// <summary>
        /// Ctor for Text writer with specific filename
        /// </summary>
        /// <param name="filename"></param>
        public AsyncTextWriter(string filename)
        {
            filename.ThrowIfNull();

            _filename = filename;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~AsyncTextWriter()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Initialize the writer to a filename
        /// </summary>
        /// <param name="filename">the name of the file to contain the contents</param>
        public void Init(string filename)
        {
            filename.ThrowIfNull();

            _filename = filename;
        }

        /// <summary>
        /// Write content to the file without a CR/LF
        /// </summary>
        /// <param name="content">the content to write</param>
        /// <param name="formatArgs">args for string.format</param>
        public void Write(string content, params object[] formatArgs)
        {
            string cmdContent = formatArgs != null & formatArgs.Length > 0 ? string.Format(content, formatArgs) : content;
            WriteCommand cmd = new WriteCommand()
            {
                WriteType = WriteTypes.NoLine,
                Content = cmdContent
            };
            Spooler.AddItem(cmd);
        }

        /// <summary>
        /// Write a line of text to the file
        /// </summary>
        /// <param name="contentLine">the line of text</param>
        /// <param name="formatArgs">args for string.format</param>
        public void WriteLine(string contentLine, params object[] formatArgs)
        {
            string cmdContent = formatArgs != null & formatArgs.Length > 0 ? string.Format(contentLine, formatArgs) : contentLine;
            WriteCommand cmd = new WriteCommand()
            {
                WriteType = WriteTypes.Line,
                Content = cmdContent
            };
            Spooler.AddItem(cmd);
        }
        #endregion

        #region Privates
        private void WriteContent(WriteCommand writeCommand)
        {
            lock (lockObj)
            {
                using (StreamWriter sw = new StreamWriter(Filename, true))
                {
                    if (writeCommand.WriteType == WriteTypes.Line)
                    {
                        sw.WriteLine(writeCommand.Content);
                    }
                    else
                    {
                        sw.Write(writeCommand.Content);
                    }
                }
            }
        }
        #endregion

        #region InnerClasses
        private enum WriteTypes
        {
            Line,
            NoLine
        }
        private class WriteCommand
        {
            public WriteTypes WriteType { get; set; }
            public string Content { get; set; }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Dispose of this text writer
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_spooler != null)
                    {
                        _spooler.ItemSpooled -= WriteContent;
                        _spooler.Dispose();
                    }
                }


                disposedValue = true;
            }
        }


        /// <summary>
        /// Dispose this text writer
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
