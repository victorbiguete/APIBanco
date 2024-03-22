using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;
using APIBanco.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Data.Entity.Infrastructure;

namespace APIBanco.Controller;

[ApiController]
[Route(template: "api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly ClientService _clientService;
    private readonly IMapper _mapper;

    public ClientsController(ClientService clientService, IMapper mapper)
    {
        _clientService = clientService;
        _mapper = mapper;
    }

    /// <summary>
    /// HTTP GET endpoint to retrieve clients.
    /// </summary>
    /// <param name="id">ID of the client.</param>
    /// <param name="cpf">CPF of the client.</param>
    /// <returns>List of ClientResponseDto objects.</returns>
    /// <response code="401">Unauthorized</response>
    /// <response code="200">OK</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(type: typeof(ClientResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ClientResponseDto>>> Get(
        [FromQuery(Name = "id")] int? id = null,
        [FromQuery(Name = "cpf")] string? cpf = null)
    {
        try
        {
            if (id == null && cpf == null)
            {
                IEnumerable<Client>? clients = await _clientService.GetAsync();
                IEnumerable<ClientResponseDto>? responses = _mapper.Map<IEnumerable<ClientResponseDto>>(source: clients);
                return Ok(new ApiTaskSuccess
                {
                    Content = responses
                });
            }

            Client? client = null;
            if (id != null)
            {
                client = await _clientService.GetByIdAsync(Id: (int)id);
            }
            else if (cpf != null)
            {
                client = await _clientService.GetByCpfAsync(Cpf: cpf);
            }

            ClientResponseDto? response = _mapper.Map<ClientResponseDto>(source: client);

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
    /// HTTP PUT endpoint to update a client by its ID.
    /// </summary>
    /// <param name="id">ID of the client to update.</param>
    /// <param name="client">Client data to update.</param>
    /// <returns>Updated client data.</returns>
    /// <response code="401">Unauthorized</response>
    /// <response code="202">Accepted</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    [HttpPut(template: "id/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(type: typeof(ClientResponseDto), statusCode: StatusCodes.Status202Accepted)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientResponseDto>> Put(
        int id,
        [FromBody] ClientRequestUpdateDto client)
    {
        try
        {
            Client? newClient = _mapper.Map<Client>(source: client);

            newClient = await _clientService.UpdateAsync(Client: newClient, Id: id);

            ClientResponseDto? response = _mapper.Map<ClientResponseDto>(source: newClient);

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

    /// <summary>
    /// Registers a new client.
    /// </summary>
    /// <param name="client">The client request data.</param>
    /// <returns>The created client response data and a token.</returns>
    /// <response code="201">Created</response>
    /// <response code="400">Bad Request</response>
    /// <response code="409">Conflict</response>
    [HttpPost("register")]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClientResponseDto>> Register(
        [FromBody] ClientRequestDto client)
    {
        try
        {
            Client? newClient = _mapper.Map<Client>(source: client);

            await _clientService.CreateAsync(Client: newClient);

            ClientResponseDto? response = _mapper.Map<ClientResponseDto>(source: newClient);

            string? token = await _clientService.LoginAsync(client: new ClientLoginRequestDto
            {
                Cpf = newClient.Cpf,
                Password = newClient.Password
            });

            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: new ApiTaskLoginTokenResponse
            {
                Token = token
            });
        }
        catch (DbUpdateException e)
        {
            return Conflict(new ApiTaskErrors
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
    /// HTTP POST endpoint to login a client.
    /// </summary>
    /// <param name="login">The client login data.</param>
    /// <returns>A token to be used in subsequent requests.</returns>
    /// <response code="200">OK</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost(template: "login")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] ClientLoginRequestDto login)
    {
        try
        {
            var response = await _clientService.LoginAsync(client: login);
            return Ok(new ApiTaskLoginTokenResponse
            {
                Token = response
            });
        }
        catch (KeyNotFoundException e)
        {
            return Unauthorized(new ApiTaskErrors
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
