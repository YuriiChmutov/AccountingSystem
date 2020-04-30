using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccountingNotebook.Controllers;
using AccountingNotebook.Models;
using System;
using AccountingNotebook.Service.AccountService;
using System.Linq;
using Moq;
using AccountingNotebook.Abstractions;

namespace AccountingNotibook.MsTest
{
    public class AccountControllerTest
    {
        public void IndexReturnsAViewResultWithAListOfAccounts()
        {
            // Arrange
            var mock = new Mock<ITransactionService>();
            //mock.Setup(repo => repo.GetUserTransactionsAsync()).(GetTestUsers());
            //var controller = new TransactionsController(mock.Object);

            //// Act
            //var result = controller.Index();

            //// Assert
            //var viewResult = Assert.IsType<ViewResult>(result);
            //var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);
            //Assert.Equal(GetTestUsers().Count, model.Count());
        }
    }
}
