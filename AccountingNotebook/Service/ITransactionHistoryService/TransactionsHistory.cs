using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingNotebook.Service.ITransactionServiceFolder
{
    public class TransactionsHistory: ITransactionHistoryService<Transaction>
    {
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
