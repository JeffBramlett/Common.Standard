using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Generic
{
    public interface IPoolItem: IDisposable
    {
        bool IsActive { get; }

        void Activate();

        void Deactivate();

    }
}
