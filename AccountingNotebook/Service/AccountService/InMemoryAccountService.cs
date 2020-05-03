using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingNotebook.Service.AccountService
{
    public class InMemoryAccountService: IAccountService
    {
        private readonly ConcurrentBag<Account> _accounts = new ConcurrentBag<Account>();
        
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

        public Task DeleteAccountAsync(Account account)
        {
            // todo: fix
            {
                _accounts.ToList().Remove(account);
                return Task.CompletedTask;
            }
        }

        public Task UpdateAccountBalanceAsync(Guid accountId, decimal balance)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account != null)
            {
                account.Balance = balance;
            }
            return Task.CompletedTask;
        }
    }
}