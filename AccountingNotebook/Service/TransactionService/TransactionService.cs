using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccountingNotebook.Service.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionHistoryService<Transaction> _transactionHistoryService;
        private readonly static SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public TransactionService(ILogger<TransactionService> logger,
            IAccountService accountService,
            ITransactionHistoryService<Transaction> transactionHistoryService)
        {
            _logger = logger;
            _accountService = accountService;
            _transactionHistoryService = transactionHistoryService;
        }

        public Transaction CreateTransaction(
            TypeOfTransaction typeOfTransaction,
            Guid idAccountFrom,
            Guid idAccountTo,
            string description,
            decimal amount)
        {
            return new Transaction(typeOfTransaction, idAccountFrom, idAccountTo, description, amount);
        }

        public async Task CreditAsync(
            Guid accountFromId,
            Guid accountToId,
            decimal amount,
            string transactionDescription)
        {
            var isFundsWereTransferedSuccessfully = false;
            var balanceBeforeTransfer = 0m;

            try
            {
                var accountFrom = await _accountService.GetAccountByIdAsync(accountFromId);

                if(accountFrom == null)
                {
                    throw new Exception($"Accont with id {accountFrom.AccountId} returned null reference");
                }

                balanceBeforeTransfer = accountFrom.Balance;

                semaphore.Wait();

                if (accountFrom.Balance - amount < 0)
                {
                    throw new Exception("Not enough funds in the account!");
                }

                await _accountService.UpdateAccountBalanceAsync(accountFromId, accountFrom.Balance - amount);

                isFundsWereTransferedSuccessfully = true;

                var transaction = CreateTransaction(TypeOfTransaction.Credit,
                    accountFromId, accountToId, transactionDescription, amount);

                await _transactionHistoryService.AddAsync(transaction);

                semaphore.Release();
            }
            catch (Exception ex)
            {
                if (isFundsWereTransferedSuccessfully)
                {
                    await _accountService.UpdateAccountBalanceAsync(accountFromId, balanceBeforeTransfer);
                }

                _logger.LogInformation($"{ex.Message}");
            }
        }

        public async Task DebitAsync(
            Guid accountFromId,
            Guid accountToId,
            decimal amount,
            string transactionDescription)
        {
            var isFundsWereTransferedSuccessfully = false;
            var balanceBeforeTransfer = 0m;

            try
            {
                var accountTo = await _accountService.GetAccountByIdAsync(accountToId);

                if (accountTo == null)
                {
                    throw new Exception($"Accont with id {accountTo.AccountId} returned null reference");
                }

                balanceBeforeTransfer = accountTo.Balance;

                semaphore.Wait();

                await _accountService.UpdateAccountBalanceAsync(accountToId, accountTo.Balance + amount);

                isFundsWereTransferedSuccessfully = true;

                var transaction = CreateTransaction(
                    TypeOfTransaction.Debit,
                    accountFromId,
                    accountToId,
                    transactionDescription,
                    amount);

                await _transactionHistoryService.AddAsync(transaction);
                semaphore.Release();
            }
            catch (Exception ex)
            {
                if (isFundsWereTransferedSuccessfully)
                {
                    await _accountService.UpdateAccountBalanceAsync(accountToId, balanceBeforeTransfer);
                }
                _logger.LogInformation(ex.Message);
            }
        }

        public async Task<List<Transaction>> GetUserTransactionsAsync(
            Guid idAccount,
            SortField sortField,
            SortDirection sortDirection,
            int pageSize,
            int pageNumber) // todo: add filter object
        {
            try
            {
                var listOfUserTransactionsToReturn = await _transactionHistoryService.GetAllAsync(idAccount);

                if(sortField == SortField.Name && sortDirection == SortDirection.Descending)
                {
                    listOfUserTransactionsToReturn = listOfUserTransactionsToReturn.OrderByDescending(t => t.TransactionId);
                }

                if (sortField == SortField.Name && sortDirection == SortDirection.Ascending)
                {
                    listOfUserTransactionsToReturn = listOfUserTransactionsToReturn.OrderBy(t => t.TransactionId);
                }

                if(sortField == SortField.Date && sortDirection == SortDirection.Ascending)
                {
                    listOfUserTransactionsToReturn = listOfUserTransactionsToReturn.OrderBy(t => t.Timestamp);
                }

                if(sortField == SortField.Date && sortDirection == SortDirection.Descending)
                {
                    listOfUserTransactionsToReturn = listOfUserTransactionsToReturn.OrderByDescending(t => t.Timestamp);
                }

                if (pageSize > 0)
                {
                    return listOfUserTransactionsToReturn.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }

                return listOfUserTransactionsToReturn.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception($"{ex.Message}");
            }
        }
    }
}