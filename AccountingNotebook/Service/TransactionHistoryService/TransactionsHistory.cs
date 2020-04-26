using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingNotebook.Service.TransactionService
{
    // todo: naming and add word service
    public class TransactionsHistory: ITransactionHistoryService<Transaction>
    {
        private readonly List<Transaction> _transactions = new List<Transaction>();

        // todo: accountId, transactionId
        public Task<Transaction> GetByIdAsync(Guid idTransaction, Guid idAccount)
        {
            // todo: change to method syntax
            var transaction = from t in _transactions
                             where (t.ToAccountId == idAccount || t.FromAccountId == idAccount) ||
                             t.TransactionId == idTransaction
                             select t;

            return Task.FromResult(transaction.FirstOrDefault());
        }

        public Task<IEnumerable<Transaction>> GetAllAsync(Guid idAccount)
        {
            var transactions = from t in _transactions
                               where t.FromAccountId == idAccount ||
                               t.ToAccountId == idAccount
                               select t;
            return Task.FromResult(_transactions.AsEnumerable());            
        }

        public Task AddAsync(Transaction transaction)        
        {
            _transactions.Add(transaction);
            return Task.CompletedTask;
        }
        
        // todo: kill it please
        public Task RemoveAsync(Transaction transaction)
        {
            _transactions.Remove(transaction);
            return Task.CompletedTask;
        }

        //кажется я ломаю логику независимости транзакций
        //хотя soft delete...
        public Task CleanAllUserTransactionsAsync(Guid idAccount) 
        {
            foreach (var transaction in _transactions)
            {
                if(transaction.ToAccountId == idAccount || transaction.FromAccountId == idAccount)
                {
                    _transactions.Remove(transaction);
                }
            }
            return Task.CompletedTask;
        }
    }
}
