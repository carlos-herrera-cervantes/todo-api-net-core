using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using TodoApiNet.Extensions;
using TodoApiNet.Models;
using TodoApiNet.Repositories;

namespace TodoApiNet.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public LoginController(IUserRepository userRepository, IConfiguration configuration, IStringLocalizer<SharedResources> localizer) => 
            (_userRepository, _configuration, _localizer) = (userRepository, configuration, localizer);

        /// <summary>
        /// POST
        /// </summary>

        #region snippet_Login

        [HttpPost]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            var token = await GetToken(credentials);

            if (token is false) return NotFound(new { Message = _localizer["InvalidCredentials"].Value });

            return Ok(token);
        }

        #endregion

        /// <summary>
        /// HELPERS
        /// </summary>

        #region snippet_GetToken
        
        public async Task<dynamic> GetToken(Credentials credentials)
        {
            var isValidCredentials = await ValidateCredentials(credentials);

            if (isValidCredentials is false) return false;

            var claims = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, credentials.Email) });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey"))), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(createdToken);
        }

        #endregion

        #region snippet_ValidateCredentials

        public async Task<bool> ValidateCredentials(Credentials credentials)
        {
            var user = await GetUserByEmail(credentials.Email);

            if (user is false) return false;

            var isValidPassword = credentials.Password == user.Password;

            if (isValidPassword) return true;

            return false;
        }

        #endregion

        #region snippet_GetUserByEmail

        public async Task<dynamic> GetUserByEmail(string email)
        {
            var filter = QueryObject<User>.CreateObjectQuery($"Email-{email}");
            var user = await _userRepository.GetOneAsync(filter);

            if (user is null) return false;

            return user;
        }

        #endregion
    }
}