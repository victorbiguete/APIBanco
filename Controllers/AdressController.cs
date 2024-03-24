using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using APIBanco.Domain.Dtos;
using APIBanco.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using APIBanco.Domain.Models.DbContext;
using APIBanco.Domain.Models.ApiTaskResponses;
using APIBanco.Domain.Models.Exceptions;

namespace APIBanco.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AdressController : ControllerBase
{
    private readonly AdressService _adressService;
    private readonly JwtService _jwtService;
    private readonly IMapper _mapper;

    public AdressController(AdressService adressService, JwtService jwtService, IMapper mapper)
    {
        _adressService = adressService;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    /// <summary>
    /// HTTP GET endpoint to retrieve adresses.
    /// </summary>
    /// <param name="id">ID of the adress.</param>
    /// <param name="cpf">CPF of the owner of the adress.</param>
    /// <returns>List of AdressResponseDto objects or a single AdressResponseDto object.</returns>
    /// <response code="401">Unauthorized</response>
    /// <response code="200">OK</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(type: typeof(ApiTaskAdressesResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiTaskAdressesResponse>> Get(
        [FromQuery(Name = "id")] int? id = null,
        [FromQuery(Name = "cpf")] string? cpf = null)
    {
        try
        {
            if (id == null && cpf == null)
            {
                IEnumerable<Adress>? adresses = await _adressService.GetAsync();
                IEnumerable<AdressResponseDto>? responses = _mapper.Map<IEnumerable<AdressResponseDto>>(source: adresses);
                return Ok(new ApiTaskAdressesResponse
                {
                    Content = responses
                });
            }

            Adress? adress = null;
            if (id != null)
            {
                adress = await _adressService.GetByIdAsync(id: (int)id);
            }
            else if (cpf != null)
            {
                adress = await _adressService.GetByCpfAsync(cpf: cpf);
            }

            AdressResponseDto? response = _mapper.Map<AdressResponseDto>(source: adress);
            return Ok(new ApiTaskAdressesResponse
            {
                Content = new List<AdressResponseDto> { response }
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
    /// HTTP PUT endpoint to update an adress.
    /// </summary>
    /// <param name="id">ID of the adress to update.</param>
    /// <param name="adress">AdressRequestDto object with the new adress data.</param>
    /// <returns>AdressResponseDto object with the updated adress data.</returns>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="202">Accepted</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    [HttpPut(template: "{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(type: typeof(ApiTaskAdressesResponse), statusCode: StatusCodes.Status202Accepted)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiTaskAdressesResponse>> Put(
        int id,
        [FromBody] AdressRequestDto adress)
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
            Adress? newAdress = _mapper.Map<Adress>(source: adress);

            newAdress.Id = id;
            newAdress = await _adressService.UpdateAsync(adress: newAdress);

            AdressResponseDto? response = _mapper.Map<AdressResponseDto>(source: newAdress);
            return Accepted(new ApiTaskAdressesResponse
            {
                Content = new List<AdressResponseDto> { response }
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
