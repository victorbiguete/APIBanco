using Microsoft.AspNetCore.Mvc;

using MongoDB.Bson;
using MongoDB.Driver;
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
    public async Task<ActionResult<ClientResponseDto>> Get(string id)
    {
        try
        {
            Client? client = await _clientService.GetAsync(Id: id);
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
    public async Task<ActionResult<ClientResponseDto>> Get(int cpf)
    {
        try
        {
            Client? client = await _clientService.GetAsync(cpf: cpf);
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
        try
        {
            Client? newClient = _mapper.Map<Client>(source: client);
            Adress? newAdress = _mapper.Map<Adress>(source: client.Adress);

            await _clientService.CreateAsync(client: newClient, adress: newAdress);

            ClientResponseDto? response = _mapper.Map<ClientResponseDto>(source: newClient);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: response);
        }
        catch (MongoWriteException e)
        {
            if (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                return Conflict(error: e.Message);
            }
            return BadRequest(error: e.Message);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPut(template: "id/{id}")]
    [ProducesResponseType(type: typeof(ClientResponseDto), statusCode: StatusCodes.Status202Accepted)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientResponseDto>> Put(string id, [FromBody] ClientRequestNoCpfDto client)
    {
        try
        {
            Client? newClient = _mapper.Map<Client>(source: client);

            newClient = await _clientService.UpdateAsync(client: newClient, Id: id);

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
