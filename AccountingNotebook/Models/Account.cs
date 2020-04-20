using System;

namespace AccountingNotebook.Models
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }         
        public TransactionsHistory TransactionsHistory { get; set; }
    }
}
