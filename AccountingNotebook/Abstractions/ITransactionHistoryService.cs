using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingNotebook.Abstractions
{
    public interface ITransactionHistoryService<T> where T : Transaction
    {
        // todo: add doc
        Task<IEnumerable<T>> GetAllAsync(Guid idAccount);
        Task<T> GetByIdAsync(Guid idTransaction, Guid idAccount);
        Task AddAsync(T transaction);        
        Task RemoveAsync(T item);        
        Task CleanAllUserTransactionsAsync(Guid idAccount);
    }    
}
