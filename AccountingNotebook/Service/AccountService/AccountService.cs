using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingNotebook.Service.AccountService
{
    // todo: naming (link InMemory or smth)
    public class AccountService: IAccountService
    {
        // todo: mb some concurrent version
        private readonly List<Account> _accounts = new List<Account>();
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

        // todo: delete by id
        public Task DeleteAccountAsync(Account account)
        {
            _accounts.Remove(account);
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