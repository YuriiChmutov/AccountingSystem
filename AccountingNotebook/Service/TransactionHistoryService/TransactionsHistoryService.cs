using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using AccountingNotebook.Utils;
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
                .Where(t => t.FromAccountId == filter.AccountId ||
                            t.ToAccountId == filter.AccountId);
            
            transactions = FilterTransactions(transactions, filter);

            return Task.FromResult(transactions.AsEnumerable());
        }

        public IEnumerable<Transaction> FilterTransactions(
            IEnumerable<Transaction> collection,
            TransactionsFilter filter)
        {
            IEnumerable<Transaction> result = collection;

            if(filter.FilterOption == FilterOption.Sorting)
            {
                if (filter.SortField == SortField.Date)
                {
                    if (filter.SortDirection == SortDirection.Ascending)
                    {
                        result = collection.OrderBy(t => t.Timestamp);
                    }
                    else
                    {
                        result = collection.OrderByDescending(t => t.Timestamp);
                    }
                }

                if(filter.SortField == SortField.Price)
                {
                    if (filter.SortDirection == SortDirection.Ascending)
                    {
                        result = collection.OrderBy(t => t.Amount);
                    }
                    else
                    {
                        result = collection.OrderByDescending(t => t.Amount);
                    }
                }
            }

            if (filter.FilterOption == FilterOption.SelectDiapozon)
            {
                if (filter.AmountParams == AmountParams.One)
                {
                    if (filter.SortField == SortField.Price)
                    {
                        if(filter.SortDirection == SortDirection.Ascending)
                        {
                            if (filter.CompareDirection == CompareDirection.LessThen)
                            {
                                result = _transactions
                                    .OrderBy(t => t.Amount).Where(t => t.Amount < filter.Price);
                            }
                            if (filter.CompareDirection == CompareDirection.MoreThen)
                            {
                                result = _transactions
                                    .OrderBy(t => t.Amount).Where(t => t.Amount > filter.Price);
                            }
                            if (filter.CompareDirection == CompareDirection.Equally)
                            {
                                result = _transactions
                                    .OrderBy(t => t.Amount).Where(t => t.Amount == filter.Price);
                            }
                        }

                        if(filter.SortDirection == SortDirection.Descending)
                        {
                            if (filter.CompareDirection == CompareDirection.LessThen)
                            {
                                result = _transactions
                                    .OrderByDescending(t => t.Amount).Where(t => t.Amount < filter.Price);
                            }
                            if (filter.CompareDirection == CompareDirection.MoreThen)
                            {
                                result = _transactions
                                    .OrderByDescending(t => t.Amount).Where(t => t.Amount > filter.Price);
                            }
                            if (filter.CompareDirection == CompareDirection.Equally)
                            {
                                result = _transactions
                                    .OrderByDescending(t => t.Amount).Where(t => t.Amount == filter.Price);
                            }
                        }
                    }

                    if (filter.SortField == SortField.Date)
                    {
                        if (filter.SortDirection == SortDirection.Ascending)
                        {
                            if (filter.CompareDirection == CompareDirection.LessThen)
                            {
                                result = _transactions
                                    .OrderBy(t => t.Timestamp)
                                    .Where(t => t.Timestamp.ConvertFromUnixTimestamp() < filter.Date);
                            }
                            if (filter.CompareDirection == CompareDirection.MoreThen)
                            {
                                result = _transactions
                                    .OrderBy(t => t.Timestamp)
                                    .Where(t => t.Timestamp.ConvertFromUnixTimestamp() > filter.Date);
                            }
                            if (filter.CompareDirection == CompareDirection.Equally)
                            {
                                result = _transactions
                                    .OrderBy(t => t.Timestamp)
                                    .Where(t => t.Timestamp.ConvertFromUnixTimestamp() == filter.Date);
                            }
                        }

                        if (filter.SortDirection == SortDirection.Descending)
                        {
                            if (filter.CompareDirection == CompareDirection.LessThen)
                            {
                                result = _transactions
                                    .OrderBy(t => t.Timestamp)
                                    .Where(t => t.Timestamp.ConvertFromUnixTimestamp() < filter.Date);
                            }
                            if (filter.CompareDirection == CompareDirection.MoreThen)
                            {
                                result = _transactions
                                    .OrderBy(t => t.Timestamp)
                                    .Where(t => t.Timestamp.ConvertFromUnixTimestamp() > filter.Date);
                            }
                            if (filter.CompareDirection == CompareDirection.Equally)
                            {
                                result = _transactions
                                    .OrderBy(t => t.Timestamp)
                                    .Where(t => t.Timestamp.ConvertFromUnixTimestamp() == filter.Date);
                            }
                        }
                    }
                }

                if (filter.AmountParams == AmountParams.Two)
                {
                    if (filter.SortField == SortField.Price)
                    {
                        if (filter.SortDirection == SortDirection.Ascending)
                        {
                            result = collection
                                .OrderBy(x => x.Timestamp)
                                .Where(x => x.Amount >= filter.PriceBot
                                 && x.Amount <= filter.PriceBot);
                        }

                        if (filter.SortDirection == SortDirection.Descending)
                        {
                            result = collection
                                .OrderByDescending(x => x.Timestamp)
                                .Where(x => x.Amount >= filter.PriceBot
                                 && x.Amount <= filter.PriceBot);
                        }

                        else
                        {
                            result = collection
                                .Where(x => x.Amount >= filter.PriceBot
                                 && x.Amount <= filter.PriceBot);
                        }
                    }

                    if (filter.SortField == SortField.Date)
                    {
                        if (filter.SortDirection == SortDirection.Ascending)
                        {
                            result = collection
                                .OrderBy(x => x.Timestamp)
                                .Where(x => x.Timestamp.ConvertFromUnixTimestamp() >= filter.BotDate
                                 && x.Timestamp.ConvertFromUnixTimestamp() <= filter.TopDate);
                        }

                        if (filter.SortDirection == SortDirection.Descending)
                        {
                            result = collection
                                .OrderByDescending(x => x.Timestamp)
                                .Where(x => x.Timestamp.ConvertFromUnixTimestamp() >= filter.BotDate
                                         && x.Timestamp.ConvertFromUnixTimestamp() <= filter.TopDate);
                        }

                        else
                        {
                            result = collection
                                .Where(x => x.Timestamp.ConvertFromUnixTimestamp() >= filter.BotDate
                                         && x.Timestamp.ConvertFromUnixTimestamp() <= filter.TopDate);
                        }
                    }
                }
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
