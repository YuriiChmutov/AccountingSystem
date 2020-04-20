using AccountingNotebook.Utils;
using System;

namespace AccountingNotebook.Models
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public string TransactionDescription { get; set; }
        public decimal Cost { get; set; }
        public long Timestamp { get; set; }
        public decimal Balance { get; set; }

        public Transaction(string _description, decimal _cost, decimal _balance, Guid id)
        {
            this.Cost = _cost;
            this.Timestamp = DateTime.UtcNow.ConvertToUnixTimestamp();          
            this.TransactionDescription = _description;
            this.Balance = _balance;
            this.TransactionId = id;
        }
    }
}
