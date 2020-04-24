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
        // todo: naming (either _variable or Variable)
        private readonly List<Transaction> transactions = new List<Transaction>();

        public Task<Transaction> GetByIdAsync(Guid id)
        {
            var transaction = transactions.FirstOrDefault(x => x.TransactionId == id);
            return Task.FromResult(transaction);
        }

        public Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return Task.FromResult(transactions.AsEnumerable());            
        }

        public Task AddAsync(Transaction transaction)        
        {
            transactions.Add(transaction);
            return Task.CompletedTask;
        }
        
        public Task AddRangeAsync(IEnumerable<Transaction> transactions)
        {
            // todo: add this in other places or remove
            this.transactions.AddRange(transactions);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(Transaction transaction)
        {
            transactions.Remove(transaction);
            return Task.CompletedTask;
        }

        public Task RemoveAllAsync()
        {
            transactions.Clear();
            return Task.CompletedTask;
        }
    }
}
