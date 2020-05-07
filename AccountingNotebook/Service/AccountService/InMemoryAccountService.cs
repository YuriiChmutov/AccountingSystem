using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace AccountingNotebook.Service.AccountService
{
    public class InMemoryAccountService: IAccountService
    {
        private readonly ConcurrentDictionary<Guid, Account> _accounts =
            new ConcurrentDictionary<Guid, Account>();
        
        public Task<Account> GetAccountByIdAsync(Guid id)
        {
            var account = _accounts.FirstOrDefault(x => x.Key == id);
            return Task.FromResult(account.Value);
        }

        public Task AddNewAccountAsync(Account account)
        {
            _accounts.GetOrAdd(account.AccountId ,account);
            return Task.CompletedTask;
        }

        public Task DeleteAccountAsync(Account account)
        {
            _accounts.Remove(account.AccountId, out account);
            return Task.CompletedTask;
        }

        public Task UpdateAccountBalanceAsync(Guid accountId, decimal balance)
        {
            var account = _accounts.FirstOrDefault(a => a.Key == accountId);
            if (account.Value != null)
            {
                account.Value.Balance = balance;
            }
            return Task.CompletedTask;
        }
    }
}