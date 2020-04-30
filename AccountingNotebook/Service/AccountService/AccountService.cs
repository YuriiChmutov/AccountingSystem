using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingNotebook.Service.AccountService
{
    // todo: naming (link InMemory or smth)
    public class AccountService: IAccountService
    {
        private readonly ConcurrentBag<Account> _accounts = new ConcurrentBag<Account>();

        // todo: remove it
        public IEnumerable<Account> Accounts
        {
            get { return _accounts; }
        }

        public Task<Account> GetAccountByIdAsync(Guid id)
        {
            var account = _accounts.FirstOrDefault(x => x.AccountId == id);
            return Task.FromResult(account);
        }

        public Task AddNewAccountAsync(Account account)
        {
            _accounts.Add(account);
            return Task.CompletedTask;
        }

        public Task UpdateAccountBalanceAsync(Guid accountId, decimal balance)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountId == accountId);
            if(account != null)
            {
                account.Balance = balance;
            }
            return Task.CompletedTask;
        }
    }
}