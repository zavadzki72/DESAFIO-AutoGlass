using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Produtos.Domain.Model.Interfaces;
using Produtos.WebApi.Controllers;
using Xunit;

namespace Produtos.WebApi.Tests
{
    public class AuthorizeControllerTests
    {
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<ITokenService> _tokenService;

        private readonly AuthorizeController _authorizeController;

        public AuthorizeControllerTests()
        {
            _configuration = new Mock<IConfiguration>();
            _tokenService = new Mock<ITokenService>();

            _authorizeController = new(_configuration.Object, _tokenService.Object);
        }

        [Fact]
        public void Authorize_Ok()
        {
            //ARRANGE
            string clientCredential = Guid.NewGuid().ToString();

            _configuration.Setup(x => x["AUTH_CLIENT_CREDENTIAL"]).Returns(clientCredential);
            _tokenService.Setup(x => x.GenerateToken()).Returns("TEST_TOKEN");

            //ACTION
            var result = _authorizeController.Authorize(clientCredential);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            _configuration.Verify(x => x["AUTH_CLIENT_CREDENTIAL"], Times.Once);
            _tokenService.Verify(x => x.GenerateToken(), Times.Once);
        }

        [Fact]
        public void Authorize_InvalidSecretType()
        {
            //ARRANGE
            string clientCredential = Guid.NewGuid().ToString();
            string invalidClientCredential = "10";

            _configuration.Setup(x => x["AUTH_CLIENT_CREDENTIAL"]).Returns(clientCredential);

            //ACTION
            var result = _authorizeController.Authorize(invalidClientCredential);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<UnauthorizedObjectResult>(result);

            _configuration.Verify(x => x["AUTH_CLIENT_CREDENTIAL"], Times.Never);
            _tokenService.Verify(x => x.GenerateToken(), Times.Never);
        }

        [Fact]
        public void Authorize_InvalidSecretValue()
        {
            //ARRANGE
            string clientCredential = Guid.NewGuid().ToString();
            string invalidClientCredential = Guid.NewGuid().ToString();

            _configuration.Setup(x => x["AUTH_CLIENT_CREDENTIAL"]).Returns(clientCredential);

            //ACTION
            var result = _authorizeController.Authorize(invalidClientCredential);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<UnauthorizedObjectResult>(result);

            _configuration.Verify(x => x["AUTH_CLIENT_CREDENTIAL"], Times.Once);
            _tokenService.Verify(x => x.GenerateToken(), Times.Never);
        }
    }
}