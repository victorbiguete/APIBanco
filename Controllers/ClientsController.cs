using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;
using APIBanco.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNetCore.Authentication;

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

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(type: typeof(IEnumerable<ClientResponseDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ClientResponseDto>>> Get()
    {
        try
        {
            List<Client>? client = await _clientService.GetAsync();
            IEnumerable<ClientResponseDto>? clientResponse = _mapper.Map<IEnumerable<ClientResponseDto>>(source: client);

            return Ok(value: clientResponse);
        }
        catch (Exception e)
        {
            return BadRequest(error: e);
        }
    }

    [HttpGet(template: "id/{id}")]
    [ProducesResponseType(type: typeof(ClientResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientResponseDto>> GetById(int id)
    {
        try
        {
            Client? client = await _clientService.GetByIdAsync(Id: id);
            ClientResponseDto? response = _mapper.Map<ClientResponseDto>(source: client);

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
    [ProducesResponseType(type: typeof(ClientResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientResponseDto>> GetByCpf(ulong cpf)
    {
        try
        {
            Client? client = await _clientService.GetByCpfAsync(Cpf: cpf);
            ClientResponseDto? clientResponse = _mapper.Map<ClientResponseDto>(source: client);

            return Ok(value: clientResponse);
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

    [HttpPost("register")]
    [ProducesResponseType(type: typeof(ClientResponseDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClientResponseDto>> Register([FromBody] ClientRequestDto client)
    {
        try
        {
            Client? newClient = _mapper.Map<Client>(source: client);

            await _clientService.CreateAsync(Client: newClient);

            ClientResponseDto? response = _mapper.Map<ClientResponseDto>(source: newClient);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: response);
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
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] ClientLoginRequestDto login)
    {
        try
        {
            var response = await _clientService.LoginAsync(client: login);
            return Ok(new ApiTaskSuccess
            {
                Content = response
            });
        }
        catch (KeyNotFoundException e)
        {
            return Unauthorized(new ApiTaskErrors
            {
                Erros = new List<string> { e.Message }
            });
        }
        // catch (Exception e)
        // {
        //     return BadRequest(new ApiTaskErrors
        //     {
        //         Erros = new List<string> { e.Message }
        //     });
        // }
    }

    [HttpPut(template: "id/{id}")]
    [ProducesResponseType(type: typeof(ClientResponseDto), statusCode: StatusCodes.Status202Accepted)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientResponseDto>> Put(int id, [FromBody] ClientRequestNoCpfDto client)
    {
        try
        {
            Client? newClient = _mapper.Map<Client>(source: client);

            newClient = await _clientService.UpdateAsync(Client: newClient, Id: id);

            ClientResponseDto? response = _mapper.Map<ClientResponseDto>(source: newClient);

            return Accepted(response);
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
