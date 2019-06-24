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
        /// <summary>
        /// The PoolItem is active (true) or not active (false)
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Activate the PoolItem
        /// </summary>
        void Activate();

        /// <summary>
        /// Deactivate the PoolItem
        /// </summary>
        void Deactivate();
    }
}
