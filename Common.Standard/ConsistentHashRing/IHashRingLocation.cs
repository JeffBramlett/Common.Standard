using System;

namespace ConsistentHashRing
{
    public interface IHashRingLocation<T>
    {
        T Item { get; set; }

        uint Key { get; set; }

        event HashRingLocation<T>.ReceivedItemDelegate ReceivedItem;

        void AddToRing(T item);
    }
}