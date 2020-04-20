using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingNotebook.Service.ITransactionServiceFolder
{
    public class TransactionsHistory: ITransactionHistoryService<Transaction>
    {
        private readonly List<Transaction> transactions = new List<Transaction>();

        public Transaction GetById(Guid id)
        {
            return transactions.FirstOrDefault(x => x.TransactionId ==  id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return transactions;
        }

        public void Add(Transaction transaction)
        {
            transactions.Add(transaction);
        }

        public void AddRange(IEnumerable<Transaction> transactions)
        {
            this.transactions.AddRange(transactions);
        }

        public void Remove(Transaction transaction)
        {
            transactions.Remove(transaction);
        }

        public void RemoveAll()
        {
            this.transactions.Clear();
        }
    }
}
