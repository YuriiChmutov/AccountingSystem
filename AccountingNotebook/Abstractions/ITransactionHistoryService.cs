using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingNotebook.Abstractions
{
    public interface ITransactionHistoryService<T> where T : Transaction
    {
        /// <summary>
        /// It is an interface which responsible for account's data behavior.
        /// </summary>
        /// <param name="accountId">It is an id of account we want see information about</param>
        Task<IEnumerable<T>> GetAllAsync(Guid accountId);
        Task<T> GetByIdAsync(Guid transactionId, Guid accountId);
        Task AddAsync(T transaction);
    }
}