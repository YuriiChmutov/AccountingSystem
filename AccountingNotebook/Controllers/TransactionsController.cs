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
        private readonly AccountsList accounts;        
        private readonly ITransactionService transactionsService;
        private ILogger<TransactionsController> _logger;

        public TransactionsController(ILogger<TransactionsController> logger,
            ITransactionService transactionsService, AccountsList accounts)
        {
            this.accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));
            this.transactionsService = transactionsService ?? throw new ArgumentNullException(nameof(transactionsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult GetTransactionHistory(Guid idAccount)
        {
            try
            {
                var account = accounts.GetById(idAccount);
                return Ok(account.TransactionsHistory.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }

        [HttpGet("{id}", Name = "GetTransaction")]
        public IActionResult GetTransaction(Guid idAccount, Guid idTransaction)
        {
            try
            {
                return Ok(transactionsService.GetTransactionInfo(idAccount, idTransaction));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactionCredit(Guid idAccount,
            [FromBody]Transaction transaction)
        {
            try
            {
                var account = accounts.GetById(idAccount);

                if (account == null)
                {
                    _logger.LogInformation($"Account with id {idAccount} wasn't found");
                    return NotFound();
                }

                await Task.Run(() => transactionsService.Credit(transaction.Cost, transaction.TransactionDescription, account.AccountId));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactionDebit(Guid idAccount,
            [FromBody]Transaction transaction)
        {
            try
            {
                var account = accounts.GetById(idAccount);

                if (account == null)
                {
                    return NotFound();
                }

                await Task.Run(() => transactionsService.Debit(transaction.Cost, transaction.TransactionDescription, account.AccountId));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid idAccount, Guid idTransaction)
        {
            try
            {
                await Task.Run(() => transactionsService.DeleteTransaction(idAccount, idTransaction));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTransactionsHistory(Guid idAccount)
        {
            try
            {
                await Task.Run(() => transactionsService.DeleteAllTransactions(idAccount));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Account with id {idAccount} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }
    }
}
