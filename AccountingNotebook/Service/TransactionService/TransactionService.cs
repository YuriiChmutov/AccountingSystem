using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
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
        private readonly static Mutex mutex= new Mutex(false, "MyMutex");

        // todo: add logging and logger to all services
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
            string transactionDescription,
            TypeOfTransaction typeOfTransaction = TypeOfTransaction.Credit)
        {
            try
            {
                var accountFrom = await _accountService.GetAccountByIdAsync(accountFromId);

                // todo: exception handling
                mutex.WaitOne();
                if (accountFrom.Balance - amount < 0)
                {
                    throw new Exception("Not enough funds in the account!");
                }

                await _accountService.UpdateAccountBalanceAsync(accountFromId, accountFrom.Balance - amount);

                var transaction = CreateTransaction(typeOfTransaction, accountFromId, accountToId,
                    transactionDescription, amount);

                await _transactionHistoryService.AddAsync(transaction);
                mutex.ReleaseMutex();
            }
            catch (Exception ex)
            {
                if(accountFromId == null)
                {
                    _logger.LogInformation($"Account with id {accountFromId} returned null reference: {ex.Message}");
                }
                else if(accountToId == null)
                {
                    _logger.LogInformation($"Account with id {accountToId} returned null reference: {ex.Message}");
                }
                else
                {
                    _logger.LogInformation(ex.Message);
                }
            }
        }

        public async Task DebitAsync(
            Guid accountFromId,
            Guid accountToId,
            decimal amount,
            string transactionDescription,
            TypeOfTransaction typeOfTransaction = TypeOfTransaction.Debit)
        {
            try
            {
                var accountTo = await _accountService.GetAccountByIdAsync(accountToId);

                await _accountService.UpdateAccountBalanceAsync(accountToId, accountTo.Balance + amount);

                var transaction = CreateTransaction(
                    typeOfTransaction,
                    accountFromId,
                    accountToId,
                    transactionDescription,
                    amount);
                await _transactionHistoryService.AddAsync(transaction);
            }
            catch (Exception ex)
            {
                if (accountFromId == null)
                {
                    _logger.LogInformation($"Account with id {accountFromId} returned null reference: {ex.Message}");
                }
                else if (accountToId == null)
                {
                    _logger.LogInformation($"Account with id {accountToId} returned null reference: {ex.Message}");
                }
                else
                {
                    _logger.LogInformation(ex.Message);
                }
            }
        }

        public async Task<List<Transaction>> GetUserTransactionsAsync(
            Guid idAccount,
            string sortOrder,
            int amountOfElementsToReturn)
        {
            try
            {
                var nameSortParametr = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                var dateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var listOfUserTransactionsToReturn = await _transactionHistoryService.GetAllAsync(idAccount);

                switch (sortOrder)
                {
                    case "name_desc":
                        listOfUserTransactionsToReturn = listOfUserTransactionsToReturn.OrderByDescending(t => t.TransactionId);
                        break;
                    case "Date":
                        listOfUserTransactionsToReturn = listOfUserTransactionsToReturn.OrderBy(t => t.Timestamp);
                        break;
                    case "date_desc":
                        listOfUserTransactionsToReturn = listOfUserTransactionsToReturn.OrderByDescending(t => t.Timestamp);
                        break;
                    default:
                        listOfUserTransactionsToReturn = listOfUserTransactionsToReturn.OrderBy(t => t.TransactionId);
                        break;
                }

                if (amountOfElementsToReturn > 0)
                {
                    return listOfUserTransactionsToReturn.Take(amountOfElementsToReturn).ToList();
                }

                return listOfUserTransactionsToReturn.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return;///////???????????????????????????????
            }
        }
    }
}