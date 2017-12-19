using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WorldTripLog.Web.Models;
using WorldTripLog.Web.Models.AccountViewModels;

namespace WorldTripLog.Web.Controllers
{

    [Route("/Token")]
    public class TokenController : Controller
    {
        private readonly SignInManager<WorldTripUser> _signInManager;
        private readonly ILogger<TokenController> _logger;
        private readonly UserManager<WorldTripUser> _userManager;
        private readonly IConfiguration _config;

        public TokenController(SignInManager<WorldTripUser> signInManager, ILogger<TokenController> logger, UserManager<WorldTripUser> userManager, IConfiguration config)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _config = config;
        }

        [AllowAnonymous]
        [Route("/Token")]
        [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                            new Claim(ClaimTypes.Name, user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Secret"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                            _config["Tokens:Audience"],
                            claims,
                            expires: DateTime.Now.AddDays(7),
                            signingCredentials: creds);

                        _logger.LogInformation($"Created token for {user.UserName}");

                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
                    }
                    else return Ok(new
                    {
                        message = "token generation failed",
                        reason = "invalid login credentials"
                    });
                }
            }

            return BadRequest("Could not create token");
        }
    }
}
