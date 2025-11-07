using System.Collections.Generic;

namespace ReadingList.Application.Interfaces
{
    public interface IRepository<T, TKey>
    {
        bool Add(T item);
        bool Upsert(T item);
        bool TryGet(TKey id, out T? item);
        IEnumerable<T> All();
    }
}
