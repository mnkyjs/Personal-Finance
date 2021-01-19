using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Personal.Finance.Application.Interface;
using Personal.Finance.Domain.Dtos;
using Personal.Finance.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Personal.Finance.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : BaseController
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public TransactionController(ILogger<TransactionController> logger, IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpPost(Name = "PostTransaction")]
        public async Task<ActionResult<TransactionDto>> CreateTransaction([FromBody] TransactionDto transaction)
        {
            try
            {
                var newTransaction = TransactionDto.MapToEntity(transaction);
                newTransaction.User = CurrentUser().Result;
                _unitOfWork.Transactions.Create(newTransaction);
                await _unitOfWork.CompleteAsync().ConfigureAwait(false);
                return StatusCode(200, TransactionDto.MapToDto(newTransaction));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to save new transaction to Database: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteTransaction")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            try
            {
                await _unitOfWork.Transactions.Delete(id).ConfigureAwait(false);
                await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to delete transaction to Database: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet(Name = "GetAllTransactions")]
        public async Task<ActionResult<IQueryable<TransactionDto>>> GetAllTransactions()
        {
            IEnumerable<Transaction> result = await _unitOfWork.Transactions
                .FindListConditionAsync(filter: u => u.UserId == CurrentUser().Result.Id).ConfigureAwait(false);
            return StatusCode(200, result.Select(TransactionDto.MapToDto).ToList().OrderBy(o => o.Date));
        }

        [HttpGet("{id}", Name = "GetSingleTransactionById")]
        public async Task<ActionResult<IQueryable<TransactionDto>>> GetSingleTransactionById(int id)
        {
            return StatusCode(200,
                TransactionDto.MapToDto(await _unitOfWork.Transactions
                    .FindByConditionSingleAsync(filter: u => u.Id == id && u.UserId == CurrentUser().Result.Id)
                    .ConfigureAwait(false)));
        }

        [HttpPut("{id}", Name = "PutTransaction")]
        public async Task<ActionResult<TransactionDto>> UpdateTransaction(int id, [FromBody] TransactionDto transaction)
        {
            try
            {
                var result = await _unitOfWork.Transactions
                    .FindByConditionSingleAsync(filter: u => u.Id == id && u.UserId == CurrentUser().Result.Id)
                    .ConfigureAwait(false);

                result = TransactionDto.MapToEntity(transaction);

                _unitOfWork.Transactions.Update(result);
                await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                return StatusCode(200, transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to update transaction to Database: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("dateRange",Name = "GetAllTransactionsInDateRange")]
        public async Task<ActionResult<IQueryable<TransactionDto>>> GetAllTransactionsInDateRange(
            [FromBody] DateRangeDto dateRangeDto)
        {
            try
            {
                var result = await _unitOfWork.Transactions
                    .FindListConditionAsync(filter: u => u.UserId == CurrentUser().Result.Id).ConfigureAwait(false);

                return StatusCode(200,
                    result.Where(t =>
                            t.Date >= dateRangeDto.StartDate && t.Date <= dateRangeDto.EndDate)
                        .Select(TransactionDto.MapToDto).ToList().OrderBy(o => o.Date));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error accord: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        private async Task<User> CurrentUser() =>
            await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}