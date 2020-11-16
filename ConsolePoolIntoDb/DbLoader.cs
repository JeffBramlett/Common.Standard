using Common.Standard.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Common.Standard;

namespace ConsolePoolIntoDb
{
    public class DbLoader : IPoolItem
    {
        DateTime start;
        DateTime end;

        private SqlConnection _connection;

        SlidingTimer _slidingTimer;

        GenericSpooler<IList<string>> _spooler;

        private GenericSpooler<IList<string>> Spooler
        {
            get
            {
                if(_spooler == null)
                {
                    _spooler = new GenericSpooler<IList<string>>();
                    _spooler.ItemSpooled += _spooler_ItemSpooled;
                    _spooler.ExceptionEncountered += _spooler_ExceptionEncountered;
                    _spooler.SpoolerEmpty += _spooler_SpoolerEmpty;
                }

                return _spooler;
            }
        }

        public string Name { get; set; }

        public int InputCount { get; set; }

        public int CtCount { get; set; }

        private SlidingTimer SlidingTimer
        {
            get
            {
                if(_slidingTimer == null)
                {
                    _slidingTimer = new SlidingTimer(TimeSpan.FromMilliseconds(100), 1, () => 
                    {
                        var avg = (end - start).TotalMilliseconds / InputCount;

                        Console.WriteLine($"{Name} ended for {InputCount} items in {(end - start).TotalMilliseconds}: avg={avg}: ConnCtor: {CtCount}");
                     });

                    CtCount++;
                }

                return _slidingTimer;
            }
        }

        private void _spooler_SpoolerEmpty()
        {
            end = DateTime.Now;

            //Console.WriteLine($"{end - start}");

            SlidingTimer.BumpTimer();
        }

        private void _spooler_ExceptionEncountered(object sender, Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        private void _spooler_ItemSpooled(IList<string> items)
        {
            InputCount++;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "usp_Table25Columns_insert";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = Connection;

                for (var i = 0; i < items.Count; i++)
                {
                    cmd.Parameters.AddWithValue($"column{i + 1}", items[i]);
                }

                while (Connection.State != System.Data.ConnectionState.Open)
                {
                    Connection.Open();
                }

                cmd.ExecuteNonQuery();
            }
            ActiveCount--;

            Console.WriteLine($"Insert completed and active count is now {ActiveCount}");
        }

        private bool IsActivated { get; set; }

        private string ConnectionString { get; set; }

        private SqlConnection Connection
        {
            get
            {
                if(_connection == null)
                {
                    _connection = new SqlConnection(ConnectionString);
                }

                return _connection;
            }
        }

        public bool IsActive { get; private set; }

        public int ActiveCount { get; set; }

        public int MaxCount { get; set; }

        public DbLoader()
        {

        }

        public DbLoader(string name)
        {
            Name = name;
        }

        public DbLoader(string name, string connectionString):
            this(name)
        {
            ConnectionString = connectionString;
        }

        public void LoadIntoDatabase(IList<string> strings)
        {
            Spooler.AddItem(strings);
            ActiveCount++;

            Console.WriteLine("Spooled");
        }

        public void Activate(params object[] startupObjects)
        {
            start = DateTime.Now;

            if (!IsActivated)
            {
                ConnectionString = startupObjects[0].ToString();
                IsActivated = true;
            }

            Console.WriteLine("Activated");

            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;

            Console.WriteLine("De-Activated");

        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_spooler != null)
                    {
                        _spooler.ItemSpooled -= _spooler_ItemSpooled;
                        _spooler.ExceptionEncountered -= _spooler_ExceptionEncountered;
                        _spooler.SpoolerEmpty -= _spooler_SpoolerEmpty;

                        _spooler.Dispose();
                    }

                    if(_connection != null)
                    {
                        _connection.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DbLoader()
        // {
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
