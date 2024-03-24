using Microsoft.AspNetCore.Mvc;

using APIBanco.Domain.Dtos;
using APIBanco.Services;
using AutoMapper;
using APIBanco.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using APIBanco.Domain.Models.DbContext;
using APIBanco.Domain.Models.ApiTaskResponses;
using APIBanco.Domain.Models.Exceptions;


namespace APIBanco.Controllers;

[ApiController]
[Route(template: "api/[controller]")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionsService _transactionsService;
    private readonly JwtService _jwtService;
    private readonly IMapper _mapper;

    public TransactionsController(TransactionsService transactionsService, JwtService jwtService, IMapper mapper)
    {
        _transactionsService = transactionsService;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    /// <summary>
    /// HTTP GET endpoint to retrieve transactions.
    /// </summary>
    /// <param name="Cpf">CPF of the client.</param>
    /// <param name="Id">ID of the transaction.</param>
    /// <param name="LessThenDays">Number of days in the past to retrieve transactions.</param>
    /// <param name="GreaterThenDays">Number of days in the past to retrieve transactions.</param>
    /// <returns>List of TransactionResponseDto objects.</returns>
    /// <response code="401">Unauthorized</response>
    /// <response code="200">OK</response>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(type: typeof(ApiTaskTransactionsResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiTaskTransactionsResponse>> Get(
        [FromQuery(Name = "transactiontype")] int? TransactionType = null,
        [FromQuery(Name = "cpf")] string? Cpf = null,
        [FromQuery(Name = "id")] int? Id = null,
        [FromQuery(Name = "lessthendays")] int? LessThenDays = null,
        [FromQuery(Name = "greaterthendays")] int? GreaterThenDays = null)
    {
        try
        {
            if (TransactionType != null)
            {
                IEnumerable<Transactions>? list = await _transactionsService.GetByType(type: (TransactionType)TransactionType);
                if (list == null)
                {
                    return NoContent();
                }
                IEnumerable<TransactionResponseDto>? transactions = _mapper.Map<IEnumerable<TransactionResponseDto>>(source: list);
                return Ok(new ApiTaskTransactionsResponse
                {
                    Content = transactions
                });
            }
            if (Id == null && Cpf == null)
            {
                if (LessThenDays == null)
                {
                    IEnumerable<Transactions>? list = await _transactionsService.GetAsync();
                    if (list == null)
                    {
                        return NoContent();
                    }
                    IEnumerable<TransactionResponseDto>? transactions = _mapper.Map<IEnumerable<TransactionResponseDto>>(source: list);
                    return Ok(new ApiTaskTransactionsResponse
                    {
                        Content = transactions
                    });
                }
                else
                {
                    int aux = (int)(LessThenDays * -1);
                    DateTime GreaterThen = DateTime.Now.AddDays(value: aux);

                    DateTime? LessThen = null;

                    if (GreaterThenDays != null)
                    {
                        aux = (int)(GreaterThenDays * -1);
                        LessThen = DateTime.Now.AddDays(value: aux);
                    }

                    IEnumerable<Transactions>? transactions = await _transactionsService.GetAsync(GreaterThen: GreaterThen, LessThen: LessThen);
                    if (transactions == null)
                    {
                        return NoContent();
                    }
                    IEnumerable<TransactionResponseDto>? response = _mapper.Map<IEnumerable<TransactionResponseDto>>(source: transactions);
                    return Ok(new ApiTaskTransactionsResponse
                    {
                        Content = response
                    });
                }
            }
            else if (Id != null)
            {
                Transactions? transaction = await _transactionsService.GetByIdAsync(Id: (int)Id);
                if (transaction == null)
                {
                    return NotFound();
                }
                TransactionResponseDto? response = _mapper.Map<TransactionResponseDto>(source: transaction);
                return Ok(new ApiTaskTransactionsResponse
                {
                    Content = new List<TransactionResponseDto> { response }
                });
            }
            else if (Cpf != null)
            {
                if (LessThenDays == null)
                {
                    IEnumerable<Transactions>? list = await _transactionsService.GetByCpfAsync(Cpf: Cpf);
                    if (list == null)
                    {
                        return NoContent();
                    }
                    IEnumerable<TransactionResponseDto>? transactions = _mapper.Map<IEnumerable<TransactionResponseDto>>(source: list);
                    return Ok(new ApiTaskTransactionsResponse
                    {
                        Content = transactions
                    });
                }
                else
                {
                    int aux = (int)(LessThenDays * -1);
                    DateTime GreaterThen = DateTime.Now.AddDays(value: aux);
                    DateTime? LessThen = null;

                    if (GreaterThenDays != null)
                    {
                        aux = (int)(GreaterThenDays * -1);
                        LessThen = DateTime.Now.AddDays(value: aux);
                    }

                    IEnumerable<Transactions>? transactions = await _transactionsService.GetAsync(Cpf: Cpf, GreaterThen: GreaterThen, LessThen: LessThen);
                    if (transactions == null)
                    {
                        return NoContent();
                    }
                    IEnumerable<TransactionResponseDto>? response = _mapper.Map<IEnumerable<TransactionResponseDto>>(source: transactions);
                    return Ok(new ApiTaskTransactionsResponse
                    {
                        Content = response
                    });
                }
            }
            else
            {
                return BadRequest(new ApiTaskErrors
                {
                    Erros = new List<string> { "Invalid request" }
                });
            }
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
    /// This is a HTTP POST method that allows a client to make a deposit into their account.
    /// </summary>
    /// <param name="transaction">The transaction details.</param>
    /// <param name="cpf">The customer's CPF.</param>
    /// <returns>The created transaction, or a BadRequest or NotFound response.</returns>
    /// <response code="401">Unauthorized request.</response>
    /// <response code="201">The transaction was created successfully.</response>
    /// <response code="400">The transaction was not valid.</response>
    /// <response code="404">The customer's account was not found.</response>
    [HttpPost(template: "deposit/{cpf}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(type: typeof(ApiTaskTransactionsResponse), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiTaskTransactionsResponse>> PostDeposity(
        [FromBody] TransactionRequestDto transaction,
        string cpf)
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
            Transactions? newTransaction = _mapper.Map<Transactions>(source: transaction);
            newTransaction.Cpf = cpf;
            newTransaction.Type = TransactionType.Deposit;
            newTransaction.Date = DateTime.UtcNow;

            await _transactionsService.CreateAsync(Transaction: newTransaction, Source: cpf);

            TransactionResponseDto? response = _mapper.Map<TransactionResponseDto>(source: newTransaction);

            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: new ApiTaskTransactionsResponse
            {
                Content = new List<TransactionResponseDto> { response }
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
    /// This is a HTTP POST method that allows a client to make a withdrawal
    /// from a bank account.
    /// </summary>
    /// <param name="transaction">The transaction details.</param>
    /// <param name="cpf">The customer's CPF.</param>
    /// <returns>A created transaction.</returns>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden</response>
    /// <response code="201">The transaction was created successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="404">Resource not found.</response>
    [HttpPost(template: "withdraw/{cpf}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(type: typeof(ApiTaskTransactionsResponse), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiTaskTransactionsResponse>> PostWithdraw(
        [FromBody] TransactionRequestDto transaction,
        string cpf)
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
            string TokenCpf = _jwtService.GetCpfClaimToken(User: User);
            if (TokenCpf != cpf)
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
            Transactions? newTransaction = _mapper.Map<Transactions>(source: transaction);
            newTransaction.Cpf = cpf;
            newTransaction.Type = TransactionType.Withdraw;
            newTransaction.Date = DateTime.UtcNow;
            await _transactionsService.CreateAsync(Transaction: newTransaction, Source: cpf);
            TransactionResponseDto? response = _mapper.Map<TransactionResponseDto>(source: newTransaction);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: new ApiTaskTransactionsResponse
            {
                Content = new List<TransactionResponseDto> { response }
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
    /// This is a HTTP POST method that allows a client to make a transfer
    /// from one bank account to another.
    /// </summary>
    /// <param name="transaction">The transaction details.</param>
    /// <param name="cpf">The customer's CPF.</param>
    /// <param name="cpftarget">The target customer's CPF.</param>
    /// <returns>A created transaction.</returns>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden</response>
    /// <response code="201">The transaction was created successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="404">Resource not found.</response>
    [HttpPost(template: "transfer/{cpf}/{cpftarget}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(type: typeof(ApiTaskTransactionsResponse), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiTaskTransactionsResponse>> PostTransfer(
        [FromBody] TransactionRequestDto transaction,
        string cpf, string cpftarget)
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
            string TokenCpf = _jwtService.GetCpfClaimToken(User: User);
            if (TokenCpf != cpf)
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
            Transactions? newTransaction = _mapper.Map<Transactions>(source: transaction);
            newTransaction.Cpf = cpf;
            newTransaction.Type = TransactionType.TransferOutcome;
            newTransaction.Date = DateTime.UtcNow;
            await _transactionsService.CreateAsync(Transaction: newTransaction, Source: cpf, Target: cpftarget);
            TransactionResponseDto? response = _mapper.Map<TransactionResponseDto>(source: newTransaction);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: new ApiTaskTransactionsResponse
            {
                Content = new List<TransactionResponseDto> { response }
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
