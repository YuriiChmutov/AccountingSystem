using AccountingNotebook.Utils;
using System;

namespace AccountingNotebook.Models
{
    public class Transaction
    {
        public TypeOfTransaction Type { get; set; } = TypeOfTransaction.Credit;
        public Guid ToAccountId { get; set; } = new Guid();
        public Guid FromAccountId { get; set; } = new Guid();
        public Guid TransactionId { get; set; } = new Guid();
        public string TransactionDescription { get; set; } = "Transaction's description";
        public decimal Amount { get; set; } = 0;
        public long Timestamp { get; set; } = DateTime.UtcNow.ConvertToUnixTimestamp();
        

        public Transaction(
            TypeOfTransaction typeOfTransaction,
            Guid idAccountFrom,
            Guid idAccountTo,
            string _description,
            decimal _amount)
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
