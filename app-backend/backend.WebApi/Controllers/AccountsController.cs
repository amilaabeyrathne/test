using AutoMapper;
using Backend.Application.Services.Interfaces;
using Backend.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.WebApi.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet("{accountId}")]
        [ProducesResponseType(typeof(AccountResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountById(Guid accountId)
        {
            var account = await _accountService.GetAccountByIdAsync(accountId);

            if (account == null)
            {
                return NotFound("Account not found.");
            }

            var responseDto = _mapper.Map<AccountResponseModel>(account);
            return Ok(responseDto);
        }
    }
}
