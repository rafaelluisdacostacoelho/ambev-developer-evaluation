using Ambev.DeveloperEvaluation.Application.Sales.CreateSale.Commands;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, Guid>
{
    private readonly ISaleRepository _repository;

    public CreateSaleHandler(ISaleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var priceTotal = 0m;

        var sale = new Sale(Guid.NewGuid(),
                            request.SaleNumber,
                            request.SaleDate,
                            request.Customer,
                            request.Branch,
                            request.IsCancelled,
                            request.CartId,
                            request.UserId,
                            priceTotal);

        await _repository.CreateAsync(sale, cancellationToken);

        return sale.Id;
    }
}
