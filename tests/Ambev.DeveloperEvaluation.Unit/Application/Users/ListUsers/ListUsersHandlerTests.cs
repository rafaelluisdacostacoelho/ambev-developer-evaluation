using Ambev.DeveloperEvaluation.Application.Pagination;
using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.Application.Users.ListUsers.Responses;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.ListUsers;

public class ListUsersHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ListUsersQueryHandler _handler;
    private readonly Faker _faker = new Faker("pt_BR");

    public ListUsersHandlerTests()
    {
        _handler = new ListUsersQueryHandler(_userRepository, _mapper);
    }

    [Fact]
    public async Task Handle_Should_ReturnPaginatedUsers()
    {
        var paginationQuery = new PaginationQuery<ListUsersQuery, ListUserResponse>(
            pageNumber: 1,
            pageSize: 10,
            order: "Username",
            filter: new ListUsersQuery { Name = "John", Email = "john@example.com" }
        );

        var paginatedResult = GenerateFakePaginatedUsers();
        var mappedUsers = paginatedResult.Items.Select(u => GenerateFakeListUserResponse(u)).ToList();

        _userRepository.GetPaginatedAsync(
            Arg.Any<int>(),
            Arg.Any<int>(),
            Arg.Any<string>(),
            Arg.Any<CancellationToken>()
        ).Returns(paginatedResult);

        _mapper.Map<ICollection<ListUserResponse>>(paginatedResult.Items).Returns(mappedUsers);

        var result = await _handler.Handle(paginationQuery, CancellationToken.None);

        result.Should().NotBeNull();
        result.Data.Should().HaveCount(paginatedResult.Items.Count);
        result.CurrentPage.Should().Be(paginatedResult.CurrentPage);
        result.TotalPages.Should().Be(paginatedResult.TotalPages);
        result.TotalCount.Should().Be(paginatedResult.TotalItems);
    }

    private PaginatedResult<User> GenerateFakePaginatedUsers()
    {
        var users = new List<User>();

        for (int i = 0; i < 10; i++)
        {
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = _faker.Internet.UserName(),
                Email = _faker.Internet.Email(),
                Phone = _faker.Phone.PhoneNumber(),
                Password = _faker.Internet.Password(),
                Role = _faker.PickRandom<UserRole>(),
                Status = _faker.PickRandom<UserStatus>()
            });
        }

        return new PaginatedResult<User>
        {
            Items = users,
            CurrentPage = 1,
            TotalPages = 1,
            TotalItems = users.Count
        };
    }

    private ListUserResponse GenerateFakeListUserResponse(User user)
    {
        return new ListUserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Password = user.Password,
            Role = user.Role,
            Status = user.Status,
            Name = new ListUserNameInfoResponse { Firstname = _faker.Name.FirstName(), Lastname = _faker.Name.LastName() },
            Address = new ListUserAddressInfoResponse
            {
                City = _faker.Address.City(),
                Street = _faker.Address.StreetName(),
                Number = _faker.Random.Int(1, 1000),
                Zipcode = _faker.Address.ZipCode(),
                Geolocation = new ListUserAddressGeolocationInfoResponse
                {
                    Latitude = _faker.Address.Latitude(),
                    Longitude = _faker.Address.Longitude()
                }
            }
        };
    }
}
