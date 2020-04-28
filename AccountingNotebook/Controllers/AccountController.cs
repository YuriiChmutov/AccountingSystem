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
        public async Task<IActionResult> GetTransactionAsync(Guid accountId, Guid transactionId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Entered information is incorrect");
                }

                if (await _transactionsService.GetTransactionInfoAsync(accountId, transactionId) == null)
                {
                    return NotFound($"Transaction with id {transactionId} for account with id {accountId} is not found");
                }

                return Ok(await _transactionsService.GetTransactionInfoAsync(accountId, transactionId));
            }
            catch (Exception ex)
            {
                // todo: change description to more generic please
                _logger.LogInformation($"Account with id {accountId} or " +
                    $"transaction with id {transactionId} returned null reference", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionHistoryAsync(Guid accountId)
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

                return Ok(await _transactionsService.GetAllUserTransactionsAsync(accountId));
            }
            catch (Exception ex)
            {
                // todo: change description to more generic please
                _logger.LogInformation($"Account with id {accountId} returned null reference", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }
    }
}