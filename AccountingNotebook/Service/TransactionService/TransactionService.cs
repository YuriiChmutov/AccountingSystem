using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingNotebook.Service.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionHistoryService<Transaction> _transactionHistoryService;
        
        // todo: add logging and logger to all services
        public TransactionService(IAccountService accountService,
            ITransactionHistoryService<Transaction> transactionHistoryService)
        {
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
            TypeOfTransaction typeOfTransaction,
            Guid accountFromId,
            Guid accountToId,
            decimal amount,
            string transactionDescription)
        {
            var accountFrom = await _accountService.GetAccountByIdAsync(accountFromId);

            // todo: tread safety
            // todo: exception handling

            if (accountFrom.Balance - amount < 0)
            {
                throw new Exception("Not enough funds in the account!");
            }

            // todo: remove -=
            await _accountService.UpdateAccountBalanceAsync(accountFromId, accountFrom.Balance -= amount);

            var transaction = CreateTransaction(typeOfTransaction, accountFromId, accountToId,
                transactionDescription, amount); 

            await _transactionHistoryService.AddAsync(transaction);
        }

        public async Task DebitAsync(
            TypeOfTransaction typeOfTransaction,
            Guid accountFromId,
            Guid accountToId,
            decimal amount,
            string transactionDescription) 
        {
            var accountTo = await _accountService.GetAccountByIdAsync(accountToId);

            // todo: same as above
            await _accountService.UpdateAccountBalanceAsync(accountToId, accountTo.Balance += amount);
            
            var transaction = CreateTransaction(
                typeOfTransaction,
                accountFromId,
                accountToId,
                transactionDescription,
                amount);
            await _transactionHistoryService.AddAsync(transaction);
        }
        
        // todo: add pagination and filters (and move to one api)
        public async Task<Transaction> GetTransactionInfoAsync(
            Guid idAccount,
            Guid idTransaction)
        {
            var transactionToReturn = await _transactionHistoryService.GetByIdAsync(idTransaction, idAccount);
            return transactionToReturn;
        }

        public async Task<List<Transaction>> GetAllUserTransactionsAsync(Guid idAccount)
        {
            var listOfUserTransactionsToReturn = await _transactionHistoryService.GetAllAsync(idAccount);
            return listOfUserTransactionsToReturn.ToList();
        }
    }
}