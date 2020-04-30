using AccountingNotebook.Models;
using AccountingNotebook.Service.AccountService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AccountingNotibook.MsTest
{
    // todo: use camelCase
    [TestClass]
    public class Account_test
    {
        [TestMethod]
        public void Can_Add_New_Account()
        {
            // Arrange
            Account a1 = new Account { AccountId = Guid.NewGuid(), Name = "Yura", Balance = 10M };
            Account a2 = new Account { AccountId = Guid.NewGuid(), Name = "Dima", Balance = 20M };

            AccountService service = new AccountService();

            // Act
            service.AddNewAccountAsync(a1);
            service.AddNewAccountAsync(a2);
            Account[] accounts = service.Accounts.ToArray();
            
            // Assert
            Assert.AreEqual(accounts.Length, 2);
            Assert.IsTrue(accounts.Contains(a1));
            Assert.IsTrue(accounts.Contains(a2));
        }

        [TestMethod]
        public void Can_Update_Account_Balance()
        {
            Account a1 = new Account { AccountId = Guid.Parse("user1"), Name = "Yura", Balance = 10M };
            Account a2 = new Account { AccountId = Guid.Parse("user2"), Name = "Dima", Balance = 20M };

            AccountService service = new AccountService();

            service.AddNewAccountAsync(a1);
            service.AddNewAccountAsync(a2);

            service.UpdateAccountBalanceAsync(a1.AccountId, 15M);
            service.UpdateAccountBalanceAsync(a2.AccountId, 100M);

            Account[] accounts = service.Accounts.ToArray();
            Assert.AreEqual(accounts[0].Balance, 15M);
            Assert.AreEqual(accounts[1].Balance, 100M);
        }
    }
}