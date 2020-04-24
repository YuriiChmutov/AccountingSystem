using System;
using System.Collections.Generic;

namespace AccountingNotebook.Abstractions
{
    // todo: I think you are not going to need it :)
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);                
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        void Remove(T item);
        void RemoveAll();
    }
}
