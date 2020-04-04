using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Common.Standard.Generic
{
    /// <summary>
    /// The contract for a PoolItem
    /// </summary>
    public interface IPoolItem: IDisposable
    {
        string Name { get; set; }

        /// <summary>
        /// The current execution count
        /// </summary>
        int ActiveCount { get; set; }

        /// <summary>
        /// The maximum count of unfinished executions
        /// </summary>
        int MaxCount { get; }

        /// <summary>
        /// Activate the PoolItem
        /// </summary>
        void Activate(params object[] startupObjects);

        /// <summary>
        /// Deactivate the PoolItem
        /// </summary>
        void Deactivate();
    }
}
