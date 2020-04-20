using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using System;
using System.Threading.Tasks;

namespace AccountingNotebook.BusinessLogic
{
    public class TransactionsService: ITransactionService
    {        
        AccountsList accountsList = new AccountsList();        

        public async void Credit(decimal amount, string transactionDescription, Guid idAccount)
        {
            var account = accountsList.GetById(idAccount);

            if(account.Balance - amount < 0)
            {
                throw new Exception("Not enough funds in the account!");
            }

            await Task.Run(() => account.Balance -= amount);
            Transaction transaction = new Transaction(transactionDescription, amount, account.Balance, new Guid());
            account.TransactionsHistory.Add(transaction);
        }

        public async void Debit(decimal amount, string transactionDescription, Guid idAccount) 
        {
            var account = accountsList.GetById(idAccount);
            await Task.Run(() => account.Balance += amount);
            Transaction transaction = new Transaction(transactionDescription, amount, account.Balance, new Guid());
            account.TransactionsHistory.Add(transaction);            
        }

        public Transaction GetTransactionInfo(Guid idAccount, Guid idTransaction)
        {
            var account = accountsList.GetById(idAccount);
            var transactionToReturn = account.TransactionsHistory.GetById(idTransaction);
            return transactionToReturn;
        }

        public async void DeleteTransaction(Guid idAccount, Guid idTransaction)
        {
            var account = accountsList.GetById(idAccount);
            var transaction = account.TransactionsHistory.GetById(idTransaction);
            await Task.Run(() => account.TransactionsHistory.Remove(transaction));
        }

        public async void DeleteAllTransactions(Guid idAccount)
        {
            var account = accountsList.GetById(idAccount);
            await Task.Run(() => account.TransactionsHistory.RemoveAll());
        }
    }
}
