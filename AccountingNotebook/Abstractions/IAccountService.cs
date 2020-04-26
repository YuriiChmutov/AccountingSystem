using AccountingNotebook.Models;
using System;
using System.Threading.Tasks;

namespace AccountingNotebook.Abstractions
{
    public interface IAccountService
    {
        // todo: add doc
        Task<Account> GetAccountByIdAsync(Guid id);
        Task AddNewAccountAsync(Account account);
        Task DeleteAccountAsync(Account account);
    }
}