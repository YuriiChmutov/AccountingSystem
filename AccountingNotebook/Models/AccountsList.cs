using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingNotebook.Models
{
    public class AccountsList
    {
        private readonly List<Account> accounts = new List<Account>()
        {
            new Account
            {
                AccountId = new Guid(),
                Name = "Dima",
                Balance = 1000M
            },
            new Account
            {
                AccountId = new Guid(),
                Name = "Yura",
                Balance = 100M
            }            
        };

        public Account GetById(Guid id)
        {
            return accounts.FirstOrDefault(x => x.AccountId == id);
        }
    }
}
