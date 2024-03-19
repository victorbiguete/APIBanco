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
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ClientResponseDto>>> Get()
    {
        List<Client>? client = await _clientService.GetAsync();
        List<Adress>? adress = await _adressService.GetAsync();

        IEnumerable<ClientResponseDto>? clientResponse = _mapper.Map<IEnumerable<ClientResponseDto>>(source: client);
        // for (int i = 0; i < clientResponse.Count(); i++)
        // {
        //     if (adress.ElementAt(index: i) != null)
        //         clientResponse.ElementAt(index: i).Adress = _mapper.Map<AdressResponseDto>(source: adress[index: i]);
        // }

        clientResponse = clientResponse
            .Select(selector: client =>
            {
                client.Adress = _mapper
                    .Map<AdressResponseDto>(source: adress
                    .FirstOrDefault(predicate: y => y.Cpf == client.Cpf)); return client;
            });

        return Ok(value: clientResponse);
    }

    [HttpGet(template: "{cpf}")]
    [ProducesResponseType(type: typeof(ClientResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientResponseDto>> Get(int cpf)
    {
        Client? client = await _clientService.GetAsync(cpf: cpf);
        Adress? adress = await _adressService.GetAsync(cpf: cpf);

        ClientResponseDto? clientResponse = _mapper.Map<ClientResponseDto>(source: client);
        clientResponse.Adress = _mapper.Map<AdressResponseDto>(source: adress);


        return Ok(value: clientResponse);
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
            response.Adress = _mapper.Map<AdressResponseDto>(source: newAdress);

            return CreatedAtAction(actionName: nameof(Get), routeValues: new { cpf = response.Cpf }, value: response);
        }
        catch (MongoWriteException e)
        {
            if (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                return Conflict(error: "CPF already exists");
            }
            return BadRequest(error: e);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}
