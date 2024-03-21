using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType(type: typeof(IEnumerable<TransactionResponseDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<TransactionResponseDto>>> Get([FromQuery(Name = "g")] int? g, [FromQuery(Name = "l")] int? l)
    {
        try
        {
            if (g == null)
            {
                IEnumerable<Transactions>? list = await _transactionsService.GetAsync();
                IEnumerable<TransactionResponseDto>? transactions = _mapper.Map<IEnumerable<TransactionResponseDto>>(source: list);
                return Ok(transactions);
            }
            else
            {
                int aux = (int)(g * -1);
                DateTime GreaterThen = DateTime.Now.AddDays(value: aux);
                DateTime? LessThen = null;

                if (l != null)
                {
                    aux = (int)(l * -1);
                    LessThen = DateTime.Now.AddDays(value: aux);
                }

                IEnumerable<Transactions>? transactions = await _transactionsService.GetAsync(GreaterThen: GreaterThen, LessThen: LessThen);

                IEnumerable<TransactionResponseDto>? response = _mapper.Map<IEnumerable<TransactionResponseDto>>(source: transactions);
                return Ok(response);
            }
        }
        catch (Exception e)
        {
            return BadRequest(error: e.Message);
        }
    }


    [HttpGet("id/{id}")]
    [ProducesResponseType(type: typeof(TransactionResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Transactions>> GetById(int id)
    {
        try
        {
            Transactions? response = await _transactionsService.GetByIdAsync(Id: id);
            return Ok(response);
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
    [ProducesResponseType(type: typeof(List<TransactionResponseDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TransactionResponseDto>>> GetByCpf(ulong cpf, [FromQuery(Name = "g")] int? g, [FromQuery(Name = "l")] int? l)
    {
        try
        {
            if (g == null)
            {
                IEnumerable<Transactions>? list = await _transactionsService.GetByCpfAsync(Cpf: cpf);
                IEnumerable<TransactionResponseDto>? transactions = _mapper.Map<IEnumerable<TransactionResponseDto>>(source: list);
                return Ok(transactions);
            }
            else
            {
                int aux = (int)(g * -1);
                DateTime GreaterThen = DateTime.Now.AddDays(value: aux);
                DateTime? LessThen = null;

                if (l != null)
                {
                    aux = (int)(l * -1);
                    LessThen = DateTime.Now.AddDays(value: aux);
                }

                IEnumerable<Transactions>? transactions = await _transactionsService.GetAsync(Cpf: cpf, GreaterThen: GreaterThen, LessThen: LessThen);

                IEnumerable<TransactionResponseDto>? response = _mapper.Map<IEnumerable<TransactionResponseDto>>(source: transactions);
                return Ok(response);
            }
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

    [HttpPost(template: "deposity/{cpf}")]
    [ProducesResponseType(type: typeof(TransactionResponseDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Transactions>> PostDeposity([FromBody] TransactionRequestDto transaction, ulong cpf)
    {
        try
        {
            Transactions? newTransaction = _mapper.Map<Transactions>(source: transaction);
            newTransaction.Cpf = cpf;
            newTransaction.Type = TransactionType.Deposit;
            newTransaction.Date = DateTime.Now;
            await _transactionsService.CreateAsync(Transaction: newTransaction, Source: cpf);
            TransactionResponseDto? response = _mapper.Map<TransactionResponseDto>(source: newTransaction);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: response);
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

    [HttpPost(template: "withdraw/{cpf}")]
    [ProducesResponseType(type: typeof(TransactionResponseDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Transactions>> PostWithdraw([FromBody] TransactionRequestDto transaction, ulong cpf)
    {
        try
        {
            Transactions? newTransaction = _mapper.Map<Transactions>(source: transaction);
            newTransaction.Cpf = cpf;
            newTransaction.Type = TransactionType.Withdraw;
            newTransaction.Date = DateTime.Now;
            await _transactionsService.CreateAsync(Transaction: newTransaction, Source: cpf);
            TransactionResponseDto? response = _mapper.Map<TransactionResponseDto>(source: newTransaction);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: response);
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

    [HttpPost(template: "transfer/{cpf}/{cpftarget}")]
    [ProducesResponseType(type: typeof(TransactionResponseDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Transactions>> PostTransfer([FromBody] TransactionRequestDto transaction, ulong cpf, ulong cpftarget)
    {
        try
        {
            Transactions? newTransaction = _mapper.Map<Transactions>(source: transaction);
            newTransaction.Cpf = cpf;
            newTransaction.Type = TransactionType.TransferOutcome;
            newTransaction.Date = DateTime.Now;
            await _transactionsService.CreateAsync(Transaction: newTransaction, Source: cpf, Target: cpftarget);
            TransactionResponseDto? response = _mapper.Map<TransactionResponseDto>(source: newTransaction);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: response);
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
