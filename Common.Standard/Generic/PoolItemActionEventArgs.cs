using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Event args for an Object Pool action
    /// </summary>
    /// <typeparam name="T">the object type</typeparam>
    public class PoolItemActionEventArgs<T>: EventArgs
    {
        /// <summary>
        /// The actions on a Pool Item
        /// </summary>
        public enum PoolItemActions
        {
            /// <summary>
            /// The item is active
            /// </summary>
            Activated,
            /// <summary>
            /// The item has been deactivated
            /// </summary>
            Deactivated
        }

        /// <summary>
        /// The instance of the args for the Pool Action
        /// </summary>
        public PoolItemActions PoolItemAction { get; set; }

        /// <summary>
        /// A message about the Pool Item action
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The Pool item for the action
        /// </summary>
        public T PoolItem { get; set; }

        /// <summary>
        /// The default Ctor
        /// </summary>
        /// <param name="poolItem">the PoolItem</param>
        /// <param name="poolItemAction">the action for the pool item</param>
        /// <param name="message">and messageing for the pool item</param>
        public PoolItemActionEventArgs(T poolItem, PoolItemActions poolItemAction = PoolItemActions.Activated, string message = "")
        {
            PoolItem = poolItem;
            PoolItemAction = poolItemAction;
            Message = message;
        }
    }
}
