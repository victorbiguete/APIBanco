using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using APIBanco.Domain.Dtos;
using APIBanco.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Data.Entity.Infrastructure;
using APIBanco.Domain.Models.DbContext;
using APIBanco.Domain.Models.ApiTaskResponses;
using APIBanco.Domain.Models.Exceptions;

namespace APIBanco.Controller;

[ApiController]
[Route(template: "api/[controller]")]
[Produces("application/json")]
public class ClientsController : ControllerBase
{
    private readonly ClientService _clientService;
    public readonly JwtService _jwtService;
    private readonly IMapper _mapper;

    public ClientsController(ClientService clientService, JwtService jwtService, IMapper mapper)
    {
        _clientService = clientService;
        _jwtService = jwtService;
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
    [ProducesResponseType(type: typeof(ApiTaskClientsResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiTaskClientsResponse>> Get(
        [FromQuery(Name = "id")] int? id = null,
        [FromQuery(Name = "cpf")] string? cpf = null)
    {
        try
        {
            if (id == null && cpf == null)
            {
                IEnumerable<Client>? clients = await _clientService.GetAsync();
                IEnumerable<ClientResponseDto>? responses = _mapper.Map<IEnumerable<ClientResponseDto>>(source: clients);
                return Ok(new ApiTaskClientsResponse
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

            return Ok(new ApiTaskClientsResponse
            {
                Content = new List<ClientResponseDto> { response }
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
    /// <response code="403">Forbidden</response>
    /// <response code="202">Accepted</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    [HttpPut(template: "{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(type: typeof(ApiTaskClientsResponse), statusCode: StatusCodes.Status202Accepted)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiTaskClientsResponse>> Put(
        int id,
        [FromBody] ClientRequestUpdateDto client)
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
            Client? newClient = _mapper.Map<Client>(source: client);

            newClient = await _clientService.UpdateAsync(Client: newClient, Id: id);

            ClientResponseDto? response = _mapper.Map<ClientResponseDto>(source: newClient);

            return Accepted(new ApiTaskClientsResponse
            {
                Content = new List<ClientResponseDto> { response }
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
    [ProducesResponseType(type: typeof(ApiTaskRegisterResponse), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiTaskRegisterResponse>> Register(
        [FromBody] ClientRequestDto client)
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
            Client? newClient = _mapper.Map<Client>(source: client);

            await _clientService.CreateAsync(Client: newClient);

            ClientResponseDto? response = _mapper.Map<ClientResponseDto>(source: newClient);

            string? token = await _clientService.LoginAsync(client: new ClientLoginRequestDto
            {
                Cpf = client.Cpf,
                Password = client.Password
            });

            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: new ApiTaskRegisterResponse
            {
                Token = token,
                Content = response
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
    [ProducesResponseType(type: typeof(ApiTaskLoginResponse), statusCode: StatusCodes.Status202Accepted)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiTaskLoginResponse>> Login(
        [FromBody] ClientLoginRequestDto login)
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
            var response = await _clientService.LoginAsync(client: login);
            return Accepted(new ApiTaskLoginResponse
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
