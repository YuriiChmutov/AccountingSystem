using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingNotebook.Service.TransactionService
{
    public class TransactionsService : ITransactionService
    {        
        private readonly IAccountService _accountService;
        private readonly ITransactionHistoryService<Transaction> _transactionHistoryService;

        public TransactionsService(IAccountService accountService,
            ITransactionHistoryService<Transaction> transactionHistoryService)
        {
            _accountService = accountService;
            _transactionHistoryService = transactionHistoryService;
        }

        // todo: please format like this
        public Transaction CreateTransaction(
            TypeOfTransaction typeOfTransaction,
            Guid idAccountFrom,
            Guid idAccountTo,
            string description,
            decimal cost,
            decimal balance)
        {
            return new Transaction(typeOfTransaction, idAccountFrom, idAccountTo, description, cost, balance);
        }

        public async Task CreditAsync(TypeOfTransaction typeOfTransaction, Guid idAccountFrom, Guid idAccountTo,
            decimal amount, string transactionDescription)
        {
           
            //var accountTo = _accountService.GetById(idAccountTo);
            var accountFrom = await _accountService.GetAccountByIdAsync(idAccountFrom);

            // todo: tread safety
            if (accountFrom.Balance - amount < 0)
            {
                throw new Exception("Not enough funds in the account!");
            }

            // todo: modify balance in account servie
            accountFrom.Balance -= amount;
            //accountTo.Balance += amount;
            
            var transaction = CreateTransaction(typeOfTransaction, idAccountFrom, idAccountTo,
                transactionDescription, amount, accountFrom.Balance); 

            await _transactionHistoryService.AddAsync(transaction);
        }        

        public async Task DebitAsync(TypeOfTransaction typeOfTransaction, Guid idAccountFrom, Guid idAccountTo,
            decimal amount, string transactionDescription) 
        {
            var accountTo = await _accountService.GetAccountByIdAsync(idAccountTo);

            accountTo.Balance += amount;

            var transaction = CreateTransaction(typeOfTransaction, idAccountFrom, idAccountTo,
                transactionDescription, amount, accountTo.Balance);
            await _transactionHistoryService.AddAsync(transaction);        
        }

        // todo: move to another controller
        public async Task<Transaction> GetTransactionInfoAsync(Guid idAccount, Guid idTransaction)
        {
            var transactionToReturn = await _transactionHistoryService.GetByIdAsync(idTransaction, idAccount);
            return transactionToReturn;
        }

        public async Task<List<Transaction>> GetAllUserTransactionsAsync(Guid idAccount)
        {
            var listOfUserTransactionsToReturn = await _transactionHistoryService.GetAllAsync(idAccount);
            return listOfUserTransactionsToReturn.ToList();
        }

        public async Task DeleteTransactionAsync(Guid idAccount, Guid idTransaction)
        {
            var transaction = await _transactionHistoryService.GetByIdAsync(idTransaction, idAccount);
            await _transactionHistoryService.RemoveAsync(transaction);
        }

        public async Task DeleteAllTransactionsAsync(Guid idAccount)
        {
            await _transactionHistoryService.CleanAllUserTransactionsAsync(idAccount);
        }
    }
}
