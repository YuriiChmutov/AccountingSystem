using AccountingNotebook.Utils;
using System;

namespace AccountingNotebook.Models
{
    public class Transaction
    {
        public TypeOfTransaction Type { get; set; }
        public Guid ToAccountId { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid TransactionId { get; set; }
        public string TransactionDescription { get; set; }
        public decimal Amount { get; set; }
        public long Timestamp { get; set; }
        

        public Transaction(TypeOfTransaction typeOfTransaction, Guid idAccountFrom, Guid idAccountTo,
            string _description, decimal _amount)
        {
            this.Type = typeOfTransaction;
            this.FromAccountId = idAccountFrom;
            this.ToAccountId = idAccountTo;
            this.Amount = _amount;
            this.Timestamp = DateTime.UtcNow.ConvertToUnixTimestamp();
            this.TransactionDescription = _description;
            this.TransactionId = new Guid();
        }
    }
}
