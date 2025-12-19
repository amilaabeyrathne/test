using AutoMapper;
using Backend.Application.Services.Interfaces;
using Backend.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.WebApi.Controllers
{
    [ApiController]
    [Route("transactions")]

    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        public TransactionsController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TransactionResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionRequestModel request)
        {

            var transaction = await _transactionService.CreateTransactionAsync(
                request.AccountId,
                request.Amount);

            var responseDto = _mapper.Map<TransactionResponseModel>(transaction);

            return CreatedAtAction(
                nameof(GetTransactionById),
                new { transactionId = responseDto.TransactionId },
                responseDto);

        }

        [HttpGet("{transactionId}")]
        [ProducesResponseType(typeof(TransactionResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionById(Guid transactionId)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);

            if (transaction == null)
            {
                return NotFound("Transaction not found");
            }

            var responseDto = _mapper.Map<TransactionResponseModel>(transaction);
            return Ok(responseDto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TransactionResponseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTransactions()
        {
            var result = await _transactionService.GetAllTransactionsAsync();

            var response = _mapper.Map<List<TransactionResponseModel>>(result);

            return Ok(response);
        }
    }
}
