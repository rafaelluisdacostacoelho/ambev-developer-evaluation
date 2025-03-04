using Ambev.DeveloperEvaluation.Application.Products.GetProduct.Commands;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct.Responses;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Handler for processing GetProductCommand requests
/// </summary>
public class GetProductCommandHandler : IRequestHandler<GetProductCommand, GetProductResponse>
{
    private readonly IProductRepository _ProductRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetProductHandler
    /// </summary>
    /// <param name="ProductRepository">The Product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for GetProductCommand</param>
    public GetProductCommandHandler(
        IProductRepository ProductRepository,
        IMapper mapper)
    {
        _ProductRepository = ProductRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetProductCommand request
    /// </summary>
    /// <param name="request">The GetProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The Product details if found</returns>
    public async Task<GetProductResponse> Handle(GetProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _ProductRepository.GetByIdAsync(request.Id, cancellationToken);

        return product == null
            ? throw new KeyNotFoundException($"Product with ID {request.Id} not found")
            : _mapper.Map<GetProductResponse>(product);
    }
}
