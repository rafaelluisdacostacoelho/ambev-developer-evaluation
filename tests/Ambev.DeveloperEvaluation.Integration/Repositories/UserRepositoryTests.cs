using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Repositories;

public class UserRepositoryTests
{
    [Fact]
    public async Task AddUser_ShouldPersistUserInDatabase()
    {
        var options = new DbContextOptionsBuilder<StoreDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        await using var context = new StoreDbContext(options);

        // Arrange: Criação do usuário com o objeto NameInfo preenchido corretamente
        var user = new User
        {
            Name = new NameInfo
            {
                Firstname = "Test",
                Lastname = "User"
            },
            Email = "test@user.com"
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act: Busca o usuário pelo email
        var result = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@user.com");

        // Assert: Valida que o objeto Name está correto
        result.Should().NotBeNull();
        result!.Name.Firstname.Should().Be("Test");
        result.Name.Lastname.Should().Be("User");
    }
}
