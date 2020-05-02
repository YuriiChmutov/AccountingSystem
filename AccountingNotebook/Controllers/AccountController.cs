using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
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
            SortField sortField,
            SortDirection sortDirection,
            int pageSize,
            int pageNumber)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Entered information is incorrect");
                }

                if (accountId == null)
                {
                    throw new ArgumentNullException($"{accountId} has null reference");
                }

                if (transactionId == null)
                {
                    throw new ArgumentNullException($"{transactionId} has null reference");
                }

                var userTransactions = await _transactionsService.GetUserTransactionsAsync(
                accountId, sortField, sortDirection, pageSize, pageNumber);

                if (userTransactions == null)
                {
                    return NotFound($"Transaction with id {transactionId} for account with id {accountId} is not found");
                }

                return Ok(userTransactions);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Method returned null reference: {ex.Message}", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionHistoryAsync(
            Guid accountId,
            SortField sortField,
            SortDirection sortDirection,
            int pageSize,
            int pageNumber)
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

                return Ok(await _transactionsService.GetUserTransactionsAsync(
                    accountId, sortField, sortDirection, pageSize, pageNumber));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Method returned exception: {ex.Message}", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }
        }
    }
}