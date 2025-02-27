using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
}
