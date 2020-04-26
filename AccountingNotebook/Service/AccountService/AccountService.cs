using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AccountingNotebook.Service.AccountService
{
    // todo: naming
    public class AccountService: IAccountService
    {
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

        public Task DeleteAccountAsync(Account account)
        {            
            _accounts.Remove(account);
            return Task.CompletedTask;
        }
    }
}
