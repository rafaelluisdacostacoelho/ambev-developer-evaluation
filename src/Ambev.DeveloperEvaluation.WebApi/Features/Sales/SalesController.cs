using Ambev.DeveloperEvaluation.Application.Sales.CreateSale.Commands;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[Route("api/sales")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
    {
        var saleId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetSaleAsync), new { id = saleId }, saleId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSaleAsync([FromRoute] Guid id)
    {
        // Lógica para buscar a venda
        await Task.FromResult(GetSaleAsync(id));
        return Ok();
    }
}
