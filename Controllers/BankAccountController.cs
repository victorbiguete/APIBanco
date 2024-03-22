using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;
using APIBanco.Services;
using APIBanco.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace APIBanco.Controllers;

[ApiController]
[Route(template: "api/[controller]")]
[Produces("application/json")]
public class BankAccountController : ControllerBase
{
    private readonly BankAccountService _bankAccountService;
    private readonly JwtService _jwtService;
    private readonly IMapper _mapper;

    public BankAccountController(BankAccountService bankAccountService, JwtService jwtService, IMapper mapper)
    {
        _bankAccountService = bankAccountService;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves bank accounts based on provided parameters.
    /// If no parameters are provided, it retrieves all bank accounts.
    /// If 'id' is provided, it retrieves a specific bank account by id.
    /// If 'cpf' is provided, it retrieves a specific bank account by cpf.
    /// </summary>
    /// <param name="id">The id of the bank account.</param>
    /// <param name="cpf">The cpf of the bank account owner.</param>
    /// <returns>A collection of BankAccountResponseDto objects representing the bank accounts.</returns>
    /// <response code="200">Success. Returns a collection of BankAccountResponseDto objects.</response>
    /// <response code="401">Unauthorized. The user is not authenticated.</response>
    /// <response code="404">Not found. The bank account was not found.</response>
    /// <response code="400">Bad request. There was an error in the request.</response>
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(type: typeof(BankAccountResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<BankAccountResponseDto>>> Get(
        [FromQuery(Name = "id")] int? id = null,
        [FromQuery(Name = "cpf")] string? cpf = null)
    {
        try
        {
            if (id == null && cpf == null)
            {
                IEnumerable<BankAccount>? BackAccounts = await _bankAccountService.GetAsync();
                IEnumerable<BankAccountResponseDto>? responses = _mapper.Map<IEnumerable<BankAccountResponseDto>>(source: BackAccounts);
                return Ok(new ApiTaskSuccess
                {
                    Content = responses
                });
            }
            BankAccount? BackAccount = null;
            if (id != null)
            {
                BackAccount = await _bankAccountService.GetByIdAsync(id: (int)id);
            }
            else if (cpf != null)
            {
                BackAccount = await _bankAccountService.GetByCpfAsync(Cpf: cpf);
            }

            BankAccountResponseDto? response = _mapper.Map<BankAccountResponseDto>(source: BackAccount);
            return Ok(new ApiTaskSuccess
            {
                Content = response
            });
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new ApiTaskErrors
            {
                Erros = new List<string> { e.Message }
            });
        }
        catch (Exception e)
        {
            return BadRequest(new ApiTaskErrors
            {
                Erros = new List<string> { e.Message }
            });
        }
    }

    /// <summary>
    /// Updates the status of a bank account.
    /// </summary>
    /// <param name="AcountStatus">The new status of the bank account.</param>
    /// <param name="id">The id of the bank account.</param>
    /// <returns>An Accepted result with the updated bank account in the response body, or a NotFound or BadRequest result.</returns>
    /// <response code="401">Unauthorized. The user is not authenticated.</response>
    /// <response code="403">Forbidden</response>
    /// <response code="202">Accepted. The bank account status was updated.</response>
    /// <response code="404">Not found. The bank account was not found.</response>
    /// <response code="400">Bad request. There was an error in the request.</response>
    [HttpPut(template: "{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(statusCode: StatusCodes.Status202Accepted)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(
        [FromBody] AccountStatus AcountStatus,
        int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiTaskErrors
            {
                Erros = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))
            });
        }

        try
        {
            int TokenId = _jwtService.GetIdClaimToken(User: User);
            if (TokenId != id)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }
        catch (TokenIdNotEqualsClientIdException e)
        {
            return BadRequest(new ApiTaskErrors
            {
                Erros = new List<string> { e.Message }
            });
        }

        try
        {
            BankAccount? response = await _bankAccountService.UpdateStatusAsync(id: id, status: AcountStatus);
            return Accepted(new ApiTaskSuccess
            {
                Content = response
            });
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new ApiTaskErrors
            {
                Erros = new List<string> { e.Message }
            });
        }
        catch (Exception e)
        {
            return BadRequest(new ApiTaskErrors
            {
                Erros = new List<string> { e.Message }
            });
        }
    }
}
