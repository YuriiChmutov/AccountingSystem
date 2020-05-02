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
            Guid accountToId,
            Guid accountFromId,
            [FromBody]Transaction transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Entered information is incorrect");
                }

                if (await _accountService.GetAccountByIdAsync(accountToId) == null)
                {
                    return NotFound($"Account with id {accountToId} doesn't exist");
                }

                if(await _accountService.GetAccountByIdAsync(accountFromId) == null)
                {
                    return NotFound($"Account with id {accountFromId} doesn't exist");
                }

                // todo: add unit tests :) mstest
                await _transactionsService.CreditAsync(accountFromId, accountToId,
                    transaction.Amount, transaction.TransactionDescription);
                
                return Ok(transaction.TransactionId);
            }
            catch (Exception ex)
            {
                if(accountFromId == null)
                {
                    _logger.LogInformation($"Account with id {accountFromId} returned null referance", ex);
                }
                if(accountToId == null)
                {
                    _logger.LogInformation($"Account with id {accountToId} returned null referance", ex);
                }
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

                if(await _accountService.GetAccountByIdAsync(accountToId) == null)
                {
                    return NotFound($"Account with id {accountToId} doesn't exist");
                }

                if (await _accountService.GetAccountByIdAsync(accountFromId) == null)
                {
                    return NotFound($"Account with id {accountFromId} doesn't exist");
                }

                await _transactionsService.DebitAsync(accountFromId, accountToId,
                    transaction.Amount, transaction.TransactionDescription);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Account with id {accountFromId} or {accountToId} wasn't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }
    }
}