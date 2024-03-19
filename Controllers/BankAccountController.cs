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
    [ProducesResponseType(type: typeof(List<BankAccountResponseDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<BankAccount>>> Get()
    {
        List<BankAccount>? BackAccount = await _bankAccountService.GetAsync();
        List<BankAccountResponseDto>? response = _mapper.Map<List<BankAccountResponseDto>>(source: BackAccount);
        return Ok(value: response);
    }

    [HttpGet(template: "{cpf}")]
    [ProducesResponseType(type: typeof(BankAccountResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BankAccount>> Get(int cpf)
    {
        BankAccount? BackAccount = await _bankAccountService.GetAsync(Cpf: cpf);
        BankAccountResponseDto? response = _mapper.Map<BankAccountResponseDto>(source: BackAccount);
        return Ok(value: response);
    }

    [HttpPut(template: "{cpf}")]
    public async Task<IActionResult> Put([FromBody] AccountStatus AcountStatus, int cpf)
    {
        await _bankAccountService.UpdateStatusAsync(Cpf: cpf, status: AcountStatus);

        return Accepted("Account updated");
    }

}

