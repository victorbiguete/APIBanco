using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;
using APIBanco.Services;

namespace APIBanco.Controller;

[ApiController]
[Route(template: "api/clients")]
public class ClientsController : ControllerBase
{
    private readonly ClientService _clientService;
    private readonly AdressService _adressService;

    private readonly IMapper _mapper;

    public ClientsController(ClientService clientService, AdressService adressService, IMapper mapper)
    {
        _clientService = clientService;
        _adressService = adressService;
        _mapper = mapper;
    }

    [HttpGet]
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
            Client? client = await _clientService.GetByIdAsync(id: id);
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
            Client? client = await _clientService.GetByCpfAsync(cpf: cpf);
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

    [HttpPost]
    [ProducesResponseType(type: typeof(ClientResponseDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Post([FromBody] ClientRequestDto client)
    {
        System.Console.WriteLine("chamou");
        try
        {
            Client? newClient = _mapper.Map<Client>(source: client);
            // var newClient = client;
            System.Console.WriteLine(newClient.ToString());
            await _clientService.CreateAsync(client: newClient);

            ClientResponseDto? response = _mapper.Map<ClientResponseDto>(source: newClient);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
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

            newClient = await _clientService.UpdateAsync(client: newClient, id: id);

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
