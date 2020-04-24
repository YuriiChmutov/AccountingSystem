using AccountingNotebook.Models;
using System;
using System.Threading.Tasks;

namespace AccountingNotebook.Abstractions
{
    public interface ITransactionService
    {
        // todo: add doc
        Task DebitAsync(decimal amount, string transactionDescription, Guid idAccount);
        Task CreditAsync(decimal amount, string transactionDescription, Guid idAccount);
        Task<Transaction> GetTransactionInfoAsync(Guid idAccount, Guid idTransaction);
        Task DeleteTransactionAsync(Guid idAccount, Guid idTransaction);
        Task DeleteAllTransactionsAsync(Guid idAccount);        
    }
}
