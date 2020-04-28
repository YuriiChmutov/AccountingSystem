using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AccountingNotebook.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionsService;
        private readonly ILogger<TransactionsController> _logger;
        private readonly IAccountService _accountService;

        public TransactionsController(ILogger<TransactionsController> logger,
            ITransactionService transactionsService,
            IAccountService accountService)
        {
            _transactionsService = transactionsService;
            _logger = logger;
            _accountService = accountService;
        }
        
        [HttpPost]
        [Route("Credit")]
        public async Task<IActionResult> CreateTransactionCreditAsync(
            Guid accountToId, Guid accountFromId,
            [FromBody]Transaction transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Entered information is incorrect");
                }

                if (await _accountService.GetAccountByIdAsync(accountToId) == null ||
                    await _accountService.GetAccountByIdAsync(accountFromId) == null)
                {
                    return NotFound($"Accounts with id {accountToId} or {accountFromId} don't exist");
                }

                await _transactionsService.CreditAsync(TypeOfTransaction.Credit, accountFromId, accountToId,
                    transaction.Amount, transaction.TransactionDescription);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Accounts with id {accountToId} or {accountFromId} returned null referance", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }

        [HttpPost]
        [Route("Debit")]
        public async Task<IActionResult> CreateTransactionDebitAsync(Guid accountFromId, Guid accountToId,
            [FromBody]Transaction transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Entered information is incorrect");
                }

                if (await _accountService.GetAccountByIdAsync(accountToId) == null ||
                    await _accountService.GetAccountByIdAsync(accountFromId) == null)
                {
                    return NotFound($"Accounts with id {accountToId} or {accountFromId} don't exist");
                }

                await _transactionsService.DebitAsync(TypeOfTransaction.Debit, accountFromId, accountToId,
                    transaction.Amount, transaction.TransactionDescription);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Accounts with id {accountFromId} or {accountToId} weren't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }
    }
}