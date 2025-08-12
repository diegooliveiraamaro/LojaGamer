using BCrypt.Net;
using LojaGamer.Controllers;
using LojaGamer.Data;
using LojaGamer.DTOs;
using LojaGamer.Entities;
using LojaGamer.Enums;
using LojaGamer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class UsersTests
{
    private readonly IConfiguration _config;

    public UsersTests()
    {
        // Configuração do TokenService para os testes
        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Jwt:Key", "uma-chave-muito-secreta-e-com-32-caracteres!" },
                { "Jwt:Issuer", "LojaGamerAPI" },
                { "Jwt:Audience", "LojaGamerClient" }
            })
            .Build();
    }

    /// <summary>
    /// Cria um novo contexto de banco em memória para cada teste
    /// </summary>
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"UsersTestDb_{System.Guid.NewGuid()}")
            .Options;

        return new AppDbContext(options);
    }

    /// <summary>
    /// Cria uma instância do UserService para os testes
    /// </summary>
    private UserService CreateUserService(AppDbContext context)
    {
        return new UserService(context);
    }

    /// <summary>
    /// Cria um usuário para testes com senha já criptografada
    /// </summary>
    private User CreateTestUser(string name, string email, string password, UserRole role)
    {
        return new User
        {
            Name = name,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };
    }

    [Fact]
    public async Task AddUser_ShouldAddUserToDatabase()
    {
        using var context = CreateDbContext();
        var service = CreateUserService(context);

        var user = CreateTestUser("João Silva", "joao@email.com", "123456", UserRole.User);

        await service.AddAsync(user);
        var result = await service.GetByEmailAsync(user.Email);

        Assert.NotNull(result);
        Assert.Equal("João Silva", result.Name);
        Assert.Equal(UserRole.User, result.Role);
        Assert.True(BCrypt.Net.BCrypt.Verify("123456", result.PasswordHash));
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenUserNotFound()
    {
        using var context = CreateDbContext();
        var service = CreateUserService(context);

        var result = await service.GetByEmailAsync("naoexiste@email.com");

        Assert.Null(result);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnUser_WhenCredentialsAreCorrect()
    {
        using var context = CreateDbContext();
        var service = CreateUserService(context);

        var email = "login@email.com";
        var password = "senha123";
        await service.AddAsync(CreateTestUser("Login Teste", email, password, UserRole.User));

        var result = await service.Authenticate(email, password);

        Assert.NotNull(result);
        Assert.Equal("Login Teste", result.Name);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnNull_WhenPasswordIsIncorrect()
    {
        using var context = CreateDbContext();
        var service = CreateUserService(context);

        await service.AddAsync(CreateTestUser("Erro Login", "erro@email.com", "senhaCorreta", UserRole.User));

        var result = await service.Authenticate("erro@email.com", "senhaErrada");

        Assert.Null(result);
    }

    [Fact]
    public async Task AddUser_ShouldNotAllowDuplicateEmail()
    {
        using var context = CreateDbContext();
        var service = CreateUserService(context);

        var email = "duplicado@email.com";
        await service.AddAsync(CreateTestUser("Usuário 1", email, "abc", UserRole.User));

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await service.AddAsync(CreateTestUser("Usuário 2", email, "xyz", UserRole.User));
        });
    }

    [Fact]
    public async Task AddUser_ShouldAssignCorrectRole()
    {
        using var context = CreateDbContext();
        var service = CreateUserService(context);

        var email = "admin@email.com";
        await service.AddAsync(CreateTestUser("Admin User", email, "adminpass", UserRole.Admin));

        var result = await service.GetByEmailAsync(email);

        Assert.NotNull(result);
        Assert.Equal(UserRole.Admin, result.Role);
    }
    [Fact]
    public async Task Deve_Alterar_Excluir_Usuario()
    {
        using var context = CreateDbContext();
        var service = CreateUserService(context);

        var user = new User
        {
            Email = "teste@email.com",
            PasswordHash = "hash123",
            Role = UserRole.User
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var controller = new UserController(context);

        // Alterar usuário
        var updateResult = await controller.UpdateUser(user.Id, new UpdateUserDto
        {
            Email = "novo@email.com",
            Password = "hash456",
            Role = UserRole.Admin
        }) as OkObjectResult;
        Assert.NotNull(updateResult);

        var updatedUser = updateResult.Value as User;
        Assert.Equal("novo@email.com", updatedUser.Email);
        Assert.Equal(UserRole.Admin, updatedUser.Role);

        // Excluir usuário
        var deleteResult = await controller.DeleteUser(user.Id);
        Assert.IsType<NoContentResult>(deleteResult);
    }



}
