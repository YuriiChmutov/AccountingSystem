using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingNotebook.Abstractions
{
    interface ITransactionHistoryService<T> where T : Transaction
    {
        // todo: add doc
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T transaction);
        Task AddRangeAsync(IEnumerable<T> transactions);
        Task RemoveAsync(T item);
        // todo: maybe word clean will suit it better?
        Task RemoveAllAsync();
    }    
}
