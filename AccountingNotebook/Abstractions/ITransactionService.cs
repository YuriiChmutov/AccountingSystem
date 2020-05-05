using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingNotebook.Abstractions
{
    /// <summary>
    /// It is an interface which responsible for transaction's behavior.
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Method which does debit
        /// </summary>
        /// <param name="idAccountFrom">Id of account (guid type) who is sender</param>
        /// <param name="idAccountTo">Id of account (guid type) who is recipient</param>
        /// <param name="amount">Amount of money which figurate in the transaction</param>
        /// <param name="transactionDescription">String description of transaction</param>
        Task DebitAsync(Guid idAccountFrom, Guid idAccountTo,
            decimal amount, string transactionDescription);

        /// <summary>
        /// Method which does debit
        /// </summary>
        /// <param name="idAccountFrom">Id of account (guid type) who is sender</param>
        /// <param name="idAccountTo">Id of account (guid type) who is recipient</param>
        /// <param name="amount">Amount of money which figurate in the transaction</param>
        /// <param name="transactionDescription">String description of transaction</param>
        Task CreditAsync(Guid idAccountFrom, Guid idAccountTo,
            decimal amount, string transactionDescription);

        /// <summary>
        /// Method which gives all acount`s transactions
        /// </summary>
        /// <param name="idAccount">Id of account which history should show</param>
        /// <param name="sortField">Sort parametr order</param>
        /// <param name="sortDirection">Sort order (A-Z or Z-A)</param>
        /// <param name="pageSize">Amount of elements on the page</param>
        /// <param name="pageNumber">Amount of pages</param>
        /// <returns></returns>
        Task<List<Transaction>> GetUserTransactionsAsync(Guid idAccount,
            SortField sortField, SortDirection sortDirection, int pageSize, int pageNumber);
    }
}