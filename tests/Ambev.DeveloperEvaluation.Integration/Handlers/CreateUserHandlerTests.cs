using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser.Commands;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Handlers;

public class CreateUserHandlerTests
{
    private readonly StoreDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly CreateUserHandler _handler;
    private readonly Faker _faker = new("pt_BR");

    public CreateUserHandlerTests()
    {
        // Configuração do DbContext em memória para testes
        var options = new DbContextOptionsBuilder<StoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new StoreDbContext(options);
        _userRepository = new UserRepository(_context);

        // Configuração automática do AutoMapper usando os Profiles existentes
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(ApplicationLayer).Assembly); // Carrega todos os profiles automaticamente
        });
        _mapper = mapperConfig.CreateMapper();

        // Mock do PasswordHasher
        var passwordHasherMock = new Mock<IPasswordHasher>();
        passwordHasherMock.Setup(ph => ph.HashPassword(It.IsAny<string>()))
                          .Returns((string password) => $"hashed-{password}");

        _passwordHasher = passwordHasherMock.Object;

        // Inicializa o handler com os serviços configurados
        _handler = new CreateUserHandler(_userRepository, _mapper, _passwordHasher);
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenCommandIsValid()
    {
        // Arrange: Configura o comando com a senha correta
        var command = new CreateUserCommand
        {
            Username = _faker.Internet.UserName(),
            Password = "Password123@", // Senha correta para o mock
            Phone = "27993663023",
            Email = "test@user.com",
            Status = UserStatus.Active,
            Role = UserRole.Admin,
            Name = new CreateNameInfoCommand
            {
                Firstname = "John",
                Lastname = "Doe"
            },
            Address = new CreateAddressInfoCommand
            {
                City = "São Paulo",
                Street = "Rua Exemplo",
                Number = 100,
                Zipcode = "12345-678",
                Geolocation = new CreateGeolocationInfoCommand
                {
                    Latitude = -23.55052,
                    Longitude = -46.633308
                }
            }
        };

        // Act: Executa o handler para criar o usuário
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Valida o resultado e os dados persistidos no banco
        result.Should().NotBeNull();
        result.Email.Should().Be(command.Email);

        var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == command.Email);
        userInDb.Should().NotBeNull();
        userInDb!.Username.Should().Be(command.Username);

        // Ajuste na validação da senha
        userInDb.Password.Should().Be($"hashed-{command.Password}");
    }
}
