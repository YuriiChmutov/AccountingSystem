using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AccountingNotebook.Controllers
{
    [ApiController]
    [Route("api/transactions")]
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

        [HttpGet]
        public async Task<IActionResult> GetTransactionHistoryAsync(Guid idAccount)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var account = await _accountService.GetAccountByIdAsync(idAccount);
                    if(account == null)
                    {
                        return NotFound();
                    }                    
                    return Ok(await _transactionsService.GetAllUserTransactionsAsync(idAccount));
                }
                return BadRequest();                
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Account with id {idAccount} returned null reference", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }
                       
        [HttpGet("{id}", Name = "GetTransaction")]
        public async Task<IActionResult> GetTransactionAsync(Guid idAccount, Guid idTransaction)
        {
            try
            {
                if (ModelState.IsValid || (idAccount != null && idTransaction != null))
                {
                    return Ok(await _transactionsService.GetTransactionInfoAsync(idAccount, idTransaction));
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Account with id {idAccount} or " +
                    $"transaction with id {idTransaction} returned null reference", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }

        [HttpPost]
        [Route("Credit")]
        public async Task<IActionResult> CreateTransactionCreditAsync(
            Guid idAccountTo, Guid idAccountFrom,
            [FromBody]Transaction transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToPage("CreateTransactionCreditAsync");
                }
                else
                {
                    //var account = _accountService.GetById(idAccount);
                    //if(account == null)
                    //{
                    //    return NotFound();
                    //}
                    await _transactionsService.CreditAsync(TypeOfTransaction.Credit, idAccountFrom, idAccountTo,
                        transaction.Cost, transaction.TransactionDescription);
                    return Ok();
                }                
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Accounts with id {idAccountTo} or {idAccountFrom} returned null referance", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }

        [HttpPost]
        [Route("Debit")]
        public async Task<IActionResult> CreateTransactionDebitAsync(Guid idAccountFrom, Guid idAccountTo,
            [FromBody]Transaction transaction)
        {
            try
            {
                await _transactionsService.DebitAsync(TypeOfTransaction.Debit, idAccountFrom, idAccountTo,
                    transaction.Cost, transaction.TransactionDescription);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Accounts with id {idAccountFrom} or {idAccountTo} weren't found", ex);
                return StatusCode(500, "A problem happened while handing your request");
            }            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid idAccount, Guid idTransaction)
        {
            try
            {
                if(!ModelState.IsValid || (idAccount == null || idTransaction == null))
                {
                    return NotFound();
                }
                else
                {
                    await _transactionsService.DeleteTransactionAsync(idAccount, idTransaction);
                    return Ok();
                }                
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
                // todo: validation
                await _transactionsService.DeleteAllTransactionsAsync(idAccount);
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
