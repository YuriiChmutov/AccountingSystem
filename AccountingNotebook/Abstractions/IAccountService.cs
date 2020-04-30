using AccountingNotebook.Models;
using System;
using System.Threading.Tasks;

namespace AccountingNotebook.Abstractions
{
    public interface IAccountService
    {
        /// <summary>
        /// It is an interface which responsible for account's behavior.
        /// </summary>
        /// <param name="accountId">It is an id of accont we want see infomation about</param>
        Task<Account> GetAccountByIdAsync(Guid accountId);
        Task AddNewAccountAsync(Account account);
        Task UpdateAccountBalanceAsync(Guid accountId, decimal balance);
    }
}