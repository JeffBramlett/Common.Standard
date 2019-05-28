using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Generic
{
    public class PoolItemActionEventArgs<T>: EventArgs
    {
        public enum PoolItemActions
        {
            Activated,
            Deactivated
        }

        public PoolItemActions PoolItemAction { get; set; }

        public string Message { get; set; }

        public T PoolItem { get; set; }

        public PoolItemActionEventArgs(T poolItem, PoolItemActions poolItemAction = PoolItemActions.Activated, string message = "")
        {
            PoolItem = poolItem;
            PoolItemAction = poolItemAction;
            Message = message;
        }
    }
}
