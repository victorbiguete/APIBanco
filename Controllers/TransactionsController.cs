using Microsoft.AspNetCore.Mvc;

using MongoDB.Bson;
using MongoDB.Driver;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;
using APIBanco.Services;
using AutoMapper;
using APIBanco.Domain.Enums;


namespace APIBanco.Controllers;

[ApiController]
[Route(template: "api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionsService _transactionsService;

    private readonly IMapper _mapper;

    public TransactionsController(TransactionsService transactionsService, IMapper mapper)
    {
        _transactionsService = transactionsService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(type: typeof(List<TransactionResponseDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<List<TransactionResponseDto>> Get()
    {
        List<TransactionResponseDto>? transactions = _mapper.Map<List<TransactionResponseDto>>(source: await _transactionsService.GetAsync());
        return transactions;
    }

    [HttpGet(template: "{cpf}")]
    [ProducesResponseType(type: typeof(List<TransactionResponseDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<List<TransactionResponseDto>> Get(int cpf)
    {
        List<TransactionResponseDto>? transactions = _mapper.Map<List<TransactionResponseDto>>(source: await _transactionsService.GetAsync(cpf: cpf));
        return transactions;
    }

    [HttpPost(template: "deposity/{source}")]
    [ProducesResponseType(type: typeof(TransactionResponseDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Transactions>> PostDeposity([FromBody] TransactionRequestDto transaction, int source)
    {
        try
        {
            Transactions? newTransaction = _mapper.Map<Transactions>(source: transaction);
            newTransaction.Cpf = source;
            newTransaction.Type = TransactionType.Deposit;
            newTransaction.Date = DateTime.Now;
            await _transactionsService.CreateAsync(transaction: newTransaction, source: source);
            TransactionResponseDto? response = _mapper.Map<TransactionResponseDto>(source: newTransaction);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { cpf = source }, value: response);
        }
        catch (Exception e)
        {
            return BadRequest(error: e.Message);
        }
    }

    [HttpPost(template: "withdraw/{source}")]
    [ProducesResponseType(type: typeof(TransactionResponseDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Transactions>> PostWithdraw([FromBody] TransactionRequestDto transaction, int source)
    {
        try
        {
            Transactions? newTransaction = _mapper.Map<Transactions>(source: transaction);
            newTransaction.Cpf = source;
            newTransaction.Type = TransactionType.Withdraw;
            newTransaction.Date = DateTime.Now;
            await _transactionsService.CreateAsync(transaction: newTransaction, source: source);
            TransactionResponseDto? response = _mapper.Map<TransactionResponseDto>(source: newTransaction);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { cpf = source }, value: response);
        }
        catch (Exception e)
        {
            return BadRequest(error: e.Message);
        }
    }

    [HttpPost(template: "transfer/{source}/{target}")]
    [ProducesResponseType(type: typeof(TransactionResponseDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Transactions>> PostTransfer([FromBody] TransactionRequestDto transaction, int source, int target)
    {
        try
        {
            Transactions? newTransaction = _mapper.Map<Transactions>(source: transaction);
            newTransaction.Cpf = source;
            newTransaction.Type = TransactionType.TransferOutcome;
            newTransaction.Date = DateTime.Now;
            await _transactionsService.CreateAsync(transaction: newTransaction, source: source, target: target);
            TransactionResponseDto? response = _mapper.Map<TransactionResponseDto>(source: newTransaction);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { cpf = source }, value: response);
        }
        catch (Exception e)
        {
            return BadRequest(error: e.Message);
        }
    }
}
