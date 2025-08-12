using FluentAssertions;
using LojaGamer.Entities;
using LojaGamer.Enums;
using LojaGamer.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LojaGamer.Tests
{
    public class AuthTests
    {
        [Fact]
        public void GenerateToken_ShouldReturnValidJwtToken()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "uma-chave-muito-secreta-e-com-32-caracteres!"},
                {"Jwt:Issuer", "LojaGamer"},
                {"Jwt:Audience", "LojaGamerUsers"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            var tokenService = new TokenService(configuration);
            var user = new User
            {
                Id = 1,
                Email = "admin@example.com",
                Role = UserRole.Admin
            };

            // Act
            var token = tokenService.GenerateToken(user);

            // Assert
            token.Should().NotBeNullOrWhiteSpace();
            token.Should().Contain(".");
        }
    }
}
