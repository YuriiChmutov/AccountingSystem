using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingNotebook.Abstractions
{
    public interface ITransactionService
    {
        /// <summary>
        /// It is an interface which responsible for transaction's behavior.
        /// </summary>
        /// <param name="typeOfTransaction">There are two kinds of transactions: credit and debit</param>
        /// <param name="idAccountFrom">Id of account (guid type) who is sender</param>
        /// <param name="idAccountTo">Id of account (guid type) who is recipient</param>
        /// <param name="amount">Amount of money which figurate in the transaction</param>
        /// <param name="transactionDescription">String description of transaction</param>
        Task DebitAsync(Guid idAccountFrom, Guid idAccountTo,
            decimal amount, string transactionDescription,
            TypeOfTransaction typeOfTransaction = TypeOfTransaction.Debit);
        Task CreditAsync(Guid idAccountFrom, Guid idAccountTo,
            decimal amount, string transactionDescription,
            TypeOfTransaction typeOfTransaction = TypeOfTransaction.Credit);
        Task<List<Transaction>> GetUserTransactionsAsync(Guid idAccount, string sortOrder, int amountOfElementsToReturn);
    }
}