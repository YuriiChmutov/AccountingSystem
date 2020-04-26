using AccountingNotebook.Utils;
using System;
using System.Threading.Tasks;

namespace AccountingNotebook.Models
{
    public class Transaction
    {
        public TypeOfTransaction Type { get; set; }
        public Guid ToAccountId { get; set; }
        public Guid FromAccountId { get; set; }        
        public Guid TransactionId { get; set; }
        public string TransactionDescription { get; set; }
        public decimal Cost { get; set; }
        public long Timestamp { get; set; }
        public decimal Balance { get; set; }

        public Transaction(TypeOfTransaction typeOfTransaction, Guid idAccountFrom, Guid idAccountTo,
            string _description, decimal _cost, decimal _balance)
        {
            this.Type = typeOfTransaction;
            this.FromAccountId = idAccountFrom;
            this.ToAccountId = idAccountTo;
            this.Cost = _cost;
            this.Timestamp = DateTime.UtcNow.ConvertToUnixTimestamp();
            this.TransactionDescription = _description;
            this.Balance = _balance;
            this.TransactionId = new Guid();
        }
    }
}
