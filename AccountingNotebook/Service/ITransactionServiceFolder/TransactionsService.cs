using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Threading.Tasks;

namespace AccountingNotebook.Service.ITransactionServiceFolder
{
    public class TransactionsService: ITransactionService
    {        
        Initializer accountsList = new Initializer();
        IAccountService accountService;

        public TransactionsService(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task CreditAsync(decimal amount, string transactionDescription, Guid idAccount)
        {
            var account = accountService.GetById(idAccount);

            if(account.Balance - amount < 0)
            {
                throw new Exception("Not enough funds in the account!");
            }

            account.Balance -= amount;
            Transaction transaction = new Transaction(transactionDescription, amount, account.Balance, new Guid());
            await account.TransactionsHistory.AddAsync(transaction);
        }

        public async Task DebitAsync(decimal amount, string transactionDescription, Guid idAccount) 
        {
            var account = accountService.GetById(idAccount);
            account.Balance += amount;
            Transaction transaction = new Transaction(transactionDescription, amount, account.Balance, new Guid());
            await account.TransactionsHistory.AddAsync(transaction);            
        }

        public async Task<Transaction> GetTransactionInfoAsync(Guid idAccount, Guid idTransaction)
        {
            var account = accountService.GetById(idAccount);
            var transactionToReturn = await account.TransactionsHistory.GetByIdAsync(idTransaction);
            return transactionToReturn;
        }

        public async Task DeleteTransactionAsync(Guid idAccount, Guid idTransaction)
        {
            var account = accountService.GetById(idAccount);
            var transaction = await account.TransactionsHistory.GetByIdAsync(idTransaction);
            await account.TransactionsHistory.RemoveAsync(transaction);
        }

        public async Task DeleteAllTransactionsAsync(Guid idAccount)
        {
            var account = accountService.GetById(idAccount);
            await account.TransactionsHistory.RemoveAllAsync();
        }
    }
}
