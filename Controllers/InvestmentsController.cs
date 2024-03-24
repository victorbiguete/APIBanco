using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using APIBanco.Domain.Dtos;
using APIBanco.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using APIBanco.Domain.Models.DbContext;
using APIBanco.Domain.Models.ApiTaskResponses;

namespace APIBanco.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class InvestmentsController : ControllerBase
{
    private readonly InvestmentService _investmentService;
    private readonly IMapper _mapper;

    public InvestmentsController(InvestmentService investmentService, IMapper mapper)
    {
        _investmentService = investmentService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(type: typeof(ApiTaskInvestmentResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ApiTaskErrors), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiTaskInvestmentResponse>> Get(
        [FromQuery(Name = "id")] int? id = null,
        [FromQuery(Name = "name")] string? name = null)
    {
        IEnumerable<Investment>? investments = null;

        try
        {
            if (id == null  && name == null)
            {
                investments = await _investmentService.GetAsync();
                IEnumerable<InvestmentResponseDto>? responses = _mapper.Map<IEnumerable<InvestmentResponseDto>>(source: investments);
                return Ok(new ApiTaskInvestmentResponse
                {
                    Content = responses
                });
            }

            Investment? investment = null;
            if (id != null)
            {
                investment = await _investmentService.GetByIdAsync(id: (int)id);
            }
             else if (name != null)
            {
                investment = await _investmentService.GetByNameAsync(Name: name);
            }
        

            InvestmentResponseDto? response = _mapper.Map<InvestmentResponseDto>(source: investment);
            return Ok(new ApiTaskInvestmentResponse
            {
                Content = new List<InvestmentResponseDto> { response }
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


    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ApiTaskInvestmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiTaskErrors), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiTaskInvestmentResponse>> PostInvestment([FromBody] InvestmentRequestDto investmentDto)
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
            var investment = _mapper.Map<Investment>(investmentDto);

            var createdInvestment = await _investmentService.CreateAsync(investment);

            var responseDto = _mapper.Map<InvestmentResponseDto>(createdInvestment);

            return Created("", new ApiTaskInvestmentResponse
            {
                Content = new List<InvestmentResponseDto> { responseDto }
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
