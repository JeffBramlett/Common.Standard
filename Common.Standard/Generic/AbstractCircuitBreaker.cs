using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Status of Circuit Breaker
    /// </summary>
    public enum CircuitBreakerStatuses
    {
        /// <summary>
        /// Breaker is closed (ready for execution)
        /// </summary>
        Closed,

        /// <summary>
        /// Breaker is open (not ready for execution)
        /// </summary>
        Open,

        /// <summary>
        /// In recovery (Reset) may fail
        /// </summary>
        HalfOpen
    }

    /// <summary>
    /// Contract for CircuitBreaker 
    /// </summary>
    /// <typeparam name="T">the input type for the circuit breaker</typeparam>
    public interface ICircuitBreaker<T>: IDisposable
    {
        /// <summary>
        /// The threshold that when reached open the circuit breaker
        /// </summary>
        int Threshold { get; set; }

        /// <summary>
        /// The timespan between retries
        /// </summary>
        TimeSpan RetryTimeSpan { get; set; }

        /// <summary>
        /// The DateTimeOffset of the last attempt
        /// </summary>
        DateTimeOffset LastAttempt { get; }

        /// <summary>
        /// The status of this CircuitBreaker
        /// </summary>
        CircuitBreakerStatuses Status { get; }

        /// <summary>
        /// Execute the CircuitBreaker function
        /// </summary>
        /// <typeparam name="R">the return type</typeparam>
        /// <param name="input">the input to execute on</param>
        /// <param name="executeDelegate">the delegate to use in Circuit break</param>
        /// <returns></returns>
        Task<R> Execute<R>(T input, Func<T, R> executeDelegate);

        /// <summary>
        /// Reset the CircuitBreaker (sets status to HalfOpen)
        /// </summary>
        /// <returns>void</returns>
        Task Reset();
        
        /// <summary>
        /// Eventhandler for status change in this CircuitBreaker
        /// </summary>
        event EventHandler BreakerStatusChange;
    }

    /// <summary>
    /// Abstract Circuit Breaker
    /// </summary>
    /// <typeparam name="T">the input type for the circuit breaker</typeparam>
    public abstract class AbstractCircuitBreaker<T> : ICircuitBreaker<T>
    {
        #region Fields
        private int _threshold = 5;
        private TimeSpan _retryTimeSpan = TimeSpan.MinValue;
        private DateTimeOffset _lastDateTimeOffset;
        private CircuitBreakerStatuses _status = CircuitBreakerStatuses.Closed;
        #endregion

        #region Properties
        /// <summary>
        /// The threshold that when reached open the circuit breaker
        /// </summary>
        public int Threshold
        {
            get { return _threshold; }
            set
            {
                if(value <= 0)
                    throw new ArgumentOutOfRangeException();

                _threshold = value;
            }
        }

        /// <summary>
        /// The threshold for the circuit break
        /// </summary>
        protected int ThresholdCount { get; set; }

        /// <summary>
        /// The timespan between retries
        /// </summary>
        public TimeSpan RetryTimeSpan
        {
            get { return _retryTimeSpan; }
            set { _retryTimeSpan = value; }
        }

        /// <summary>
        /// The DateTimeOffset of the last attempt
        /// </summary>
        public DateTimeOffset LastAttempt
        {
            get { return _lastDateTimeOffset; }
            protected set { _lastDateTimeOffset = value; }
        }

        /// <summary>
        /// The status of this CircuitBreaker
        /// </summary>
        public CircuitBreakerStatuses Status
        {
            get { return _status; }
            protected set
            {
                if (_status != value)
                {
                    _status = value;
                    BreakerStatusChange?.Invoke(this, new BreakerStatusChangedEventArgs() {BreakerStatus = value});
                }
            }
        }

        #endregion

        #region Events and delegates
        /// <summary>
        /// Eventhandler for status change in this CircuitBreaker
        /// </summary>
        public event EventHandler BreakerStatusChange;
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        public AbstractCircuitBreaker()
        {
                
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~AbstractCircuitBreaker()
        {
            Dispose(false);
        }

        #endregion

        #region Publics
        /// <summary>
        /// Execute the CircuitBreaker function
        /// </summary>
        /// <typeparam name="R">the return type</typeparam>
        /// <param name="input">the input to execute on</param>
        /// <param name="executeDelegate">the delegate to use in Circuit break</param>
        /// <returns>result from execution of delegate (could be null)</returns>
        public async Task<R> Execute<R>(T input, Func<T, R> executeDelegate)
        {
            while (true)
            {
                var result = executeDelegate(input);
                if (!result.Equals(default(R)))
                {
                    Status = CircuitBreakerStatuses.Closed;
                    return result;
                }
                else
                {
                    ThresholdCount++;
                    if (ThresholdCount >= Threshold)
                    {
                        Status = CircuitBreakerStatuses.Open;
                        break;
                    }

                    await Task.Delay(RetryTimeSpan);
                }
            }

            LastAttempt = DateTimeOffset.Now;

            return default(R);
        }

        /// <summary>
        /// Reset the CircuitBreaker (sets status to HalfOpen)
        /// </summary>
        /// <returns>void</returns>
        public async Task Reset()
        {
            await Task.Run(() =>
            {
                Status = CircuitBreakerStatuses.HalfOpen;
                ThresholdCount = 0;
            });
        }
        #endregion

        #region protected Overrides
        /// <summary>
        /// Virtual for extending classes to override to dispose managed classes (base method does nothing)
        /// </summary>
        protected virtual void DisposeManaged()
        {

        }
        /// <summary>
        /// Virtual for extending classes to override to dispose unmanaged classes (base method does nothing)
        /// </summary>
        protected virtual void DisposeUnManaged()
        {

        }
        #endregion

        #region privates

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeManaged();
                }

                DisposeUnManaged();

                disposedValue = true;
            }
        }

        /// <summary>
        /// Release resources and call Dispose overrides
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
