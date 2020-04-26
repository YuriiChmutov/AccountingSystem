using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingNotebook.Abstractions
{
    public interface ITransactionService
    {
        // todo: add doc
        Task DebitAsync(TypeOfTransaction typeOfTransaction, Guid idAccountFrom, Guid idAccountTo,
            decimal amount, string transactionDescription);
        Task CreditAsync(TypeOfTransaction typeOfTransaction, Guid idAccountFrom, Guid idAccountTo,
            decimal amount, string transactionDescription);
        Task<Transaction> GetTransactionInfoAsync(Guid idAccount, Guid idTransaction);
        Task<List<Transaction>> GetAllUserTransactionsAsync(Guid idAccount);
        Task DeleteTransactionAsync(Guid idAccount, Guid idTransaction);
        Task DeleteAllTransactionsAsync(Guid idAccount);        
    }
}
