using AccountingNotebook.Models;
using System;
using System.Threading.Tasks;

namespace AccountingNotebook.Abstractions
{
    /// <summary>
    /// It is an interface which responsible for account's behavior.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// It is a method which returns account by id
        /// </summary>
        /// <param name="accountId">It is an id of accont we want see infomation about</param>
        /// <returns>One object of type Account</returns>
        Task<Account> GetAccountByIdAsync(Guid accountId);

        /// <summary>
        /// It is a method which adds new account to the collection of accounts
        /// </summary>
        /// <param name="account">It`s object which should add to the collection of accounts</param>
        Task AddNewAccountAsync(Account account);

        /// <summary>
        /// It is a method which deletes one account
        /// </summary>
        /// <param name="account">It`s object which should delete from collection of accounts</param>
        /// <returns></returns>
        Task DeleteAccountAsync(Account account);

        /// <summary>
        /// It is a method which updates balance of account
        /// </summary>
        /// <param name="accountId">Id of account which balance should update</param>
        /// <param name="balance">Value which will be new balance of accout</param>
        /// <returns></returns>
        Task UpdateAccountBalanceAsync(Guid accountId, decimal balance);
    }
}