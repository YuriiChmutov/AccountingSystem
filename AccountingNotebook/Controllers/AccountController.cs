using AccountingNotebook.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AccountingNotebook.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ITransactionService _transactionsService;
        private readonly ILogger<TransactionsController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<TransactionsController> logger,
            ITransactionService transactionsService,
            IAccountService accountService)
        {
            _transactionsService = transactionsService;
            _logger = logger;
            _accountService = accountService;
        }

        // todo: test
        [HttpGet("{id}", Name = "GetTransaction")]
        public async Task<IActionResult> GetTransactionsAsync(
            Guid accountId,
            Guid transactionId,
            string sortParam,
            int amountOfElements)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Entered information is incorrect");
                }

                // todo: move to var
                if (await _transactionsService.GetUserTransactionsAsync(accountId, sortParam, amountOfElements) == null)
                {
                    return NotFound($"Transaction with id {transactionId} for account with id {accountId} is not found");
                }

                return Ok(await _transactionsService.GetUserTransactionsAsync(accountId, sortParam, amountOfElements));
            }
            catch (Exception ex)
            {
                // todo: do validation in body ;)
                if(accountId == null)
                {
                    _logger.LogInformation($"Account with id {accountId} returned null reference: {ex.Message}", ex);
                }

                if(transactionId == null)
                {
                    _logger.LogInformation($"Transaction with id {transactionId} returned null reference: {ex.Message}", ex);
                }

                return StatusCode(500, "A problem happened while handing your request");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionHistoryAsync(
            Guid accountId,
            string sortParam,
            int amountOfTransactions)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Entered information is incorrect");
                }

                if (_accountService.GetAccountByIdAsync(accountId) == null)
                {
                    return NotFound($"User with id {accountId} is not found");
                }

                return Ok(await _transactionsService.GetUserTransactionsAsync(accountId, sortParam, amountOfTransactions));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Method returned exception: {ex.Message}", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }
    }
}