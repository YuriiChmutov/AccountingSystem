using AccountingNotebook.Models;
using System;

namespace AccountingNotebook.Abstractions
{
    public interface ITransactionService
    {
        void Debit(decimal amount, string transactionDescription, Guid idAccount);
        void Credit(decimal amount, string transactionDescription, Guid idAccount);
        Transaction GetTransactionInfo(Guid idAccount, Guid idTransaction);
        void DeleteTransaction(Guid idAccount, Guid idTransaction);
        void DeleteAllTransactions(Guid idAccount);        
    }
}
