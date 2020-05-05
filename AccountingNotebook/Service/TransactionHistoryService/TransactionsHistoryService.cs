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

        public Task<IEnumerable<Transaction>> GetAllTransactionsAsync(
            Guid accountId,
            SortField sortField,
            SortDirection sortDirection,
            int pageSize,
            int pageNumber)
        {
            var transactions = _transactions
                .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId);

            transactions = FilterTransactions(
                transactions,
                new TransactionsFilter
                {
                    SortField = sortField,
                    SortDirection = sortDirection,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });

            return Task.FromResult(transactions.AsEnumerable());
        }

        public IEnumerable<Transaction> FilterTransactions(
            IEnumerable<Transaction> collection,
            TransactionsFilter filter)
        {//глянуть внизу файла
            if(filter.SortField == SortField.Name && filter.SortDirection == SortDirection.Descending)
            {
                collection = collection.OrderByDescending(t => t.TransactionId);
            }

            if (filter.SortField == SortField.Name && filter.SortDirection == SortDirection.Ascending)
            {
                collection = collection.OrderBy(t => t.TransactionId);
            }

            if (filter.SortField == SortField.Date && filter.SortDirection == SortDirection.Ascending)
            {
                collection = collection.OrderBy(t => t.Timestamp);
            }

            if (filter.SortField == SortField.Date && filter.SortDirection == SortDirection.Descending)
            {
                collection = collection.OrderByDescending(t => t.Timestamp);
            }

            if (filter.PageSize > 0)
            {
                return collection.Skip((filter.PageNumber - 1) * filter.PageSize)
                                 .Take(filter.PageSize).ToList();
            }

            return collection;
        }

        public Task AddAsync(Transaction transaction)
        {
            _transactions.Add(transaction);
            return Task.CompletedTask;
        }
    }
}


            //string parametr = null;
            //switch (filter.SortField)
            //{
            //    case SortField.Name:
            //        collection = collection.OrderBy(t => t.TransactionId);
            //        parametr = "name";
            //        break;
            //    case SortField.Date:
            //        collection = collection.OrderBy(t => t.Timestamp);
            //        parametr = "date";
            //        break;
            //    default:
            //        collection = collection.OrderByDescending(t => t.Timestamp);
            //        parametr = String.Empty;
            //        break;
            //}

            //switch (filter.SortDirection)
            //{
            //    case SortDirection.Ascending:
            //        collection.OrderBy(c => c.)
            //        break;
            //    case SortDirection.Descending:
            //        break;
            //    default:
            //        break;
            //}
