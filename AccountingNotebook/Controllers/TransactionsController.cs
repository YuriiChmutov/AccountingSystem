using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AccountingNotebook.Controllers
{
    [ApiController]
    [Route("api/account/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly Initializer accounts;        
        private readonly ITransactionService transactionsService;
        private readonly ILogger<TransactionsController> logger;
        private readonly IAccountService accountService;

        public TransactionsController(ILogger<TransactionsController> logger,
            ITransactionService transactionsService, Initializer accounts,
            IAccountService accountService)
        {
            this.accounts = accounts;
            this.transactionsService = transactionsService;
            this.logger = logger;
            this.accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionHistoryAsync(Guid idAccount)
        {
            try
            {
                var account = accountService.GetById(idAccount);
                return Ok(await account.TransactionsHistory.GetAllAsync());
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }
                       
        [HttpGet("{id}", Name = "GetTransaction")]
        public async Task<IActionResult> GetTransactionAsync(Guid idAccount, Guid idTransaction)
        {
            try
            {
                return Ok(await transactionsService.GetTransactionInfoAsync(idAccount, idTransaction));
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }

        [HttpPost]
        [Route("Credit")]
        public async Task<IActionResult> CreateTransactionCreditAsync(Guid idAccount,
            [FromBody]Transaction transaction)
        {
            try
            {
                var account = accountService.GetById(idAccount);

                if (account == null)
                {
                    logger.LogInformation($"Account with id {idAccount} wasn't found");
                    return NotFound();
                }

                await transactionsService.CreditAsync(transaction.Cost, transaction.TransactionDescription, account.AccountId);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }

        [HttpPost]
        [Route("Debit")]
        public async Task<IActionResult> CreateTransactionDebitAsync(Guid idAccount,
            [FromBody]Transaction transaction)
        {
            try
            {
                var account = accountService.GetById(idAccount);

                if (account == null)
                {
                    return NotFound();
                }

                await transactionsService.DebitAsync(transaction.Cost, transaction.TransactionDescription, account.AccountId);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid idAccount, Guid idTransaction)
        {
            try
            {
                await transactionsService.DeleteTransactionAsync(idAccount, idTransaction);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTransactionsHistory(Guid idAccount)
        {
            try
            {
                await transactionsService.DeleteAllTransactionsAsync(idAccount);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }
    }
}
