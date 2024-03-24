using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using APIBanco.Domain.Dtos;
using APIBanco.Services;
using APIBanco.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using APIBanco.Domain.Models.ApiTaskResponses;
using APIBanco.Domain.Models.Exceptions;
using APIBanco.Domain.Models.DbContext;

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

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(type: typeof(ApiTaskBankAccountsResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiTaskBankAccountsResponse>> Get(
        [FromQuery(Name = "id")] int? id = null,
        [FromQuery(Name = "cpf")] string? cpf = null)
    {
        try
        {
            if (id == null && cpf == null)
            {
                IEnumerable<BankAccount>? BackAccounts = await _bankAccountService.GetAsync();
                IEnumerable<BankAccountResponseDto>? responses = _mapper.Map<IEnumerable<BankAccountResponseDto>>(source: BackAccounts);
                return Ok(new ApiTaskBankAccountsResponse
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
            return Ok(new ApiTaskBankAccountsResponse
            {
                Content = new List<BankAccountResponseDto> { response }
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

    [HttpPatch(template: "{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(type: typeof(ApiTaskBankAccountsResponse), statusCode: StatusCodes.Status202Accepted)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiTaskBankAccountsResponse>> Patch(
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
            BankAccount? bankAccounts = await _bankAccountService.UpdateStatusAsync(id: id, status: AcountStatus);
            BankAccountResponseDto? response = _mapper.Map<BankAccountResponseDto>(source: bankAccounts);
            return Accepted(new ApiTaskBankAccountsResponse
            {
                Content = new List<BankAccountResponseDto> { response }
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
