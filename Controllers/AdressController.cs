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
[Route("api/adress")]
public class AdressController : ControllerBase
{
    private readonly AdressService _adressService;
    private readonly IMapper _mapper;

    public AdressController(AdressService adressService, IMapper mapper)
    {
        _adressService = adressService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(type: typeof(AdressResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AdressResponseDto>>> Get()
    {
        try
        {
            IEnumerable<Adress>? adress = await _adressService.GetAsync();
            IEnumerable<AdressResponseDto>? response = _mapper.Map<IEnumerable<AdressResponseDto>>(source: adress);
            return Ok(value: response);
        }
        catch (Exception e)
        {
            return BadRequest(error: e.Message);
        }
    }
    [HttpGet(template: "id/{id}")]
    [ProducesResponseType(type: typeof(AdressResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AdressResponseDto>>> Get(string id)
    {
        try
        {
            Adress? adress = await _adressService.GetAsync(Id: id);
            AdressResponseDto? response = _mapper.Map<AdressResponseDto>(source: adress);
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
    [ProducesResponseType(type: typeof(AdressResponseDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AdressResponseDto>>> Get(int cpf)
    {
        try
        {
            IEnumerable<Adress>? adress = await _adressService.GetAsync(cpf: cpf);
            IEnumerable<AdressResponseDto>? response = _mapper.Map<IEnumerable<AdressResponseDto>>(source: adress);
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

    [HttpPost(template: "{cpf}")]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdressResponseDto>> Post(int cpf, [FromBody] AdressRequestDto adress)
    {
        try
        {
            Adress? newAdress = _mapper.Map<Adress>(source: adress);
            newAdress.Cpf = cpf;

            await _adressService.CreateAsync(adress: newAdress);
            AdressResponseDto? response = _mapper.Map<AdressResponseDto>(source: newAdress);

            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = response.Id }, value: response);
        }
        catch (Exception e)
        {
            return BadRequest(error: e.Message);
        }
    }

    [HttpPut(template: "{id}")]
    [ProducesResponseType(type: typeof(AdressResponseDto), statusCode: StatusCodes.Status202Accepted)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AdressResponseDto>> Put(string id, [FromBody] AdressRequestDto adress)
    {
        try
        {
            Adress? newAdress = _mapper.Map<Adress>(source: adress);

            newAdress.Id = id;
            newAdress = await _adressService.UpdateAsync(adress: newAdress);

            AdressResponseDto? response = _mapper.Map<AdressResponseDto>(source: newAdress);
            return Accepted(value: response);
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

    [HttpDelete(template: "{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status202Accepted)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(string id)
    {
        try
        {
            await _adressService.DeleteAsync(Id: id);
            return Accepted();
        }
        catch (Exception e)
        {
            return BadRequest(error: e.Message);
        }
    }
}
