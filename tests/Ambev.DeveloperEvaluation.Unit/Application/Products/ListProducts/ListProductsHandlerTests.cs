using Ambev.DeveloperEvaluation.Application.Pagination;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class ListProductsHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ListProductsHandler _handler;
    private readonly Faker _faker = new Faker("pt_BR");

    public ListProductsHandlerTests()
    {
        _handler = new ListProductsHandler(_productRepository, _mapper);
    }

    [Fact]
    public async Task Handle_Should_ReturnPaginatedProducts()
    {
        var paginationQuery = new PaginationQuery<ListProductsQuery, ListProductResponse>(
            pageNumber: 1,
            pageSize: 10,
            order: "Title",
            filter: new ListProductsQuery { Name = "Sample Product" }
        );

        var paginatedResult = GenerateFakePaginatedProducts();
        var mappedProducts = paginatedResult.Items.Select(p => GenerateFakeListProductResponse(p)).ToList();

        _productRepository.GetPaginatedAsync(
            Arg.Any<int>(),
            Arg.Any<int>(),
            Arg.Any<string>(),
            Arg.Any<CancellationToken>()
        ).Returns(paginatedResult);

        _mapper.Map<ICollection<ListProductResponse>>(paginatedResult.Items).Returns(mappedProducts);

        var result = await _handler.Handle(paginationQuery, CancellationToken.None);

        result.Should().NotBeNull();
        result.Data.Should().HaveCount(paginatedResult.Items.Count);
        result.CurrentPage.Should().Be(paginatedResult.CurrentPage);
        result.TotalPages.Should().Be(paginatedResult.TotalPages);
        result.TotalCount.Should().Be(paginatedResult.TotalItems);
    }

    private PaginatedResult<Product> GenerateFakePaginatedProducts()
    {
        var products = new List<Product>();

        for (int i = 0; i < 10; i++)
        {
            var newProduct = new Product(
                title: _faker.Commerce.ProductName(),
                price: _faker.Random.Decimal(10, 1000),
                description: _faker.Commerce.ProductDescription(),
                image: _faker.Image.PicsumUrl(),
                category: new CategoryInfo(
                    externalId: _faker.Random.Guid().ToString(),
                    name: _faker.Commerce.Categories(1).First()
                ),
                rating: new RatingInfo(
                    externalId: _faker.Random.Guid().ToString(),
                    averageRate: _faker.Random.Double(1, 5),
                    totalReviews: _faker.Random.Int(1, 100)
                )
            );
            products.Add(newProduct);
        }

        return new PaginatedResult<Product>
        {
            Items = products,
            CurrentPage = 1,
            TotalPages = 1,
            TotalItems = products.Count
        };
    }

    private static ListProductResponse GenerateFakeListProductResponse(Product product)
    {
        return new ListProductResponse
        {
            Title = product.Title,
            Price = product.Price,
            Description = product.Description,
            Image = product.Image,
            Category = new ListProductCategoryInfoResponse
            {
                ExternalId = product.Category.ExternalId,
                Name = product.Category.Name
            },
            Rating = new ListProductRatingInfoResponse
            {
                ExternalId = product.Rating.ExternalId,
                AverageRate = product.Rating.AverageRate,
                TotalReviews = product.Rating.TotalReviews
            }
        };
    }
}
