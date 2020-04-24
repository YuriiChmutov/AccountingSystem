using AccountingNotebook.Models;
using System;

namespace AccountingNotebook.Abstractions
{
    public interface IAccountService
    {
        // todo: add doc
        Account GetById(Guid id);
        void AddNewAccount(Account account);
        void DeleteAccount(Guid id);
    }
}



// todo: make async
//Task<Account> GetByIdAsync(Guid id);
//Task AddNewAccountAsync(Account account);
//Task DeleteAccountAsync(Guid id);