using Ambev.DeveloperEvaluation.Application.Sales.CreateSale.Commands;
using Ambev.DeveloperEvaluation.Common.Cache;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[Route("api/sales")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;

    private const string SALE_CACHE_KEY = "Product:{id}";

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
    {
        var id = await _mediator.Send(command);
        return Created(nameof(GetSaleAsync), new { id }, id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = nameof(GetSaleAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Cache(SALE_CACHE_KEY, DurationInMinutes = 15)]
    public async Task<IActionResult> GetSaleAsync([FromRoute] Guid id)
    {
        // Lógica para buscar a venda
        await Task.FromResult(GetSaleAsync(id));
        return Ok();
    }
}
