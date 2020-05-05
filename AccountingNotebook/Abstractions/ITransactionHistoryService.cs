using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingNotebook.Abstractions
{
    /// <summary>
    /// It is an interface which responsible for account's data behavior.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITransactionHistoryService<T> where T : Transaction
    {
        /// <summary>
        /// Method which give list of all user`s transactions 
        /// </summary>
        /// <param name="accountId">It is an id of account we want see transactions</param>
        /// <returns>List of user`s transactions</returns>
        Task<IEnumerable<T>> GetAllTransactionsAsync(TransactionsFilter filter);

        /// <summary>
        /// Method which give transaction of account
        /// </summary>
        /// <param name="transactionId">Which transaction should show</param>
        /// <param name="accountId">Which account want see its transaction</param>
        /// <returns>Account`s transaction</returns>
        Task<T> GetByIdAsync(Guid transactionId, Guid accountId);

        /// <summary>
        /// Method which add transaction to the transaction`s history
        /// </summary>
        /// <param name="transaction">Object transaction which should add to the history</param>
        /// <returns></returns>
        Task AddAsync(T transaction);
    }
}