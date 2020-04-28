using System;
using System.Collections.Generic;

namespace AccountingNotebook.Models
{
    public class TestData
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
    }
}
