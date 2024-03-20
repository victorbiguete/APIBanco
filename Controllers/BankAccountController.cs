using Microsoft.AspNetCore.Mvc;

using MongoDB.Bson;
using MongoDB.Driver;
using AutoMapper;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;
using APIBanco.Services;
using APIBanco.Domain.Enums;

namespace APIBanco.Controllers;

[ApiController]
[Route(template: "api/bankaccounts")]
public class BankAccountController : ControllerBase
{
    private readonly BankAccountService _bankAccountService;

    private readonly IMapper _mapper;

    public BankAccountController(BankAccountService bankAccountService, IMapper mapper)
    {
        _bankAccountService = bankAccountService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(type: typeof(IEnumerable<BankAccountResponseDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<BankAccount>>> Get()
    {
        try
        {
            IEnumerable<BankAccount>? BackAccount = await _bankAccountService.GetAsync();
            IEnumerable<BankAccountResponseDto>? response = _mapper.Map<IEnumerable<BankAccountResponseDto>>(source: BackAccount);
            return Ok(value: response);
        }
        catch (Exception e)
        {
            return BadRequest(error: e.Message);
        }
    }

    [HttpGet(template: "id/{id}")]
    [ProducesResponseType(type: typeof(BankAccountResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BankAccountResponseDto>> Get(string id)
    {
        try
        {
            BankAccount? BackAccount = await _bankAccountService.GetAsync(Id: id);
            BankAccountResponseDto? response = _mapper.Map<BankAccountResponseDto>(source: BackAccount);
            return Ok(value: response);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(error: e.Message);
        }
    }

    [HttpGet(template: "cpf/{cpf}")]
    [ProducesResponseType(type: typeof(BankAccountResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<BankAccountResponseDto>>> Get(int cpf)
    {
        try
        {
            IEnumerable<BankAccount>? BackAccount = await _bankAccountService.GetAsync(Cpf: cpf);
            IEnumerable<BankAccountResponseDto>? response = _mapper.Map<IEnumerable<BankAccountResponseDto>>(source: BackAccount);
            return Ok(value: response);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(error: e.Message);
        }
    }

    [HttpPut(template: "{cpf}")]
    [ProducesResponseType(statusCode: StatusCodes.Status202Accepted)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put([FromBody] AccountStatus AcountStatus, int cpf)
    {
        try
        {
            BankAccount? response = await _bankAccountService.UpdateStatusAsync(Cpf: cpf, status: AcountStatus);
            return Accepted(value: response);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(error: e.Message);
        }
    }

}

