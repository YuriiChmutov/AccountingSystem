using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingNotebook.Service.TransactionHistoryService
{
    public class TransactionsHistoryService: ITransactionHistoryService<Transaction>
    {
        private readonly ConcurrentBag<Transaction> _transactions = new ConcurrentBag<Transaction>();

        public Task<Transaction> GetByIdAsync(Guid transactionId, Guid accountId)
        {
            var transaction = _transactions
                .Where(t => (t.ToAccountId == accountId || t.FromAccountId == accountId) && 
                            t.TransactionId == transactionId)
                .FirstOrDefault();

            return Task.FromResult(transaction);
        }

        public Task<IEnumerable<Transaction>> GetAllTransactionsAsync(TransactionsFilter filter)
        {
            var transactions = _transactions
                .Where(t => t.FromAccountId == filter.AccountId || t.ToAccountId == filter.AccountId);

            // todo: maybe some filter that would be frequently usable
            transactions = FilterTransactions(transactions, filter);

            return Task.FromResult(transactions.AsEnumerable());
        }

        public IEnumerable<Transaction> FilterTransactions(
            IEnumerable<Transaction> collection,
            TransactionsFilter filter)
        {
            IEnumerable<Transaction> result = collection;

            // todo: add all or left date only
            if (filter.SortField == SortField.Name)
            {
                if (filter.SortDirection == SortDirection.Descending)
                {
                    result = result.OrderByDescending(t => t.TransactionId);
                }
                else
                {
                    result = result.OrderBy(t => t.TransactionId);
                }
            }

            if (filter.SortField == SortField.Date && filter.SortDirection == SortDirection.Ascending)
            {
                collection = collection.OrderBy(t => t.Timestamp);
            }

            if (filter.SortField == SortField.Date && filter.SortDirection == SortDirection.Descending)
            {
                collection = collection.OrderByDescending(t => t.Timestamp);
            }
                
            return result
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();
        }

        public Task AddAsync(Transaction transaction)
        {
            _transactions.Add(transaction);
            return Task.CompletedTask;
        }
    }
}
