using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Produtos.Domain.Model.Interfaces;

namespace Produtos.WebApi.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class AuthorizeController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthorizeController(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("client-credential")]
        [AllowAnonymous]
        public IActionResult Authorize([FromForm] string clientCredential)
        {
            if (!Guid.TryParse(clientCredential, out var secretGuid))
            {
                return Unauthorized("Invalid secret");
            }

            var secretTrueGuid = Guid.Parse(_configuration["AUTH_CLIENT_CREDENTIAL"]);

            if (secretGuid != secretTrueGuid)
                return Unauthorized("Invalid secret");

            var token = _tokenService.GenerateToken();
            return Ok(token);
        }
    }
}
