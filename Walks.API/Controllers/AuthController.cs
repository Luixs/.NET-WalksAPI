using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Walks.API.Models.DTO;
using Walks.API.Repositories;

namespace Walks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        }

        /// <summary>
        /// REGISTER A NEW USER
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RequestDto model)
        {
            var identityNewUser = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Username
            };
             
            var identityResult = await _userManager.CreateAsync(identityNewUser, model.Password);

            if (identityResult.Succeeded)
            {
                if (model.Roles != null && model.Roles.Length > 0)
                {
                    identityResult = await _userManager.AddToRolesAsync(identityNewUser, model.Roles);

                    if (identityResult.Succeeded) return Ok("User was registered! Please, login!");
                }
            }

            return BadRequest("Something went wrong!");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Username);
            if (user != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, model.Password);

                if(checkPasswordResult)
                {
                    // -- Get the roles from this user
                    var userRoles = await _userManager.GetRolesAsync(user);

                    if(userRoles != null)
                    {
                        var jwtTk = _tokenRepository.CreateJWTToken(user, userRoles.ToList());

                        var resp = new LoginResponseDto
                        {
                            JwtToken = jwtTk
                        };
                        // -- Generate a token
                        return Ok(resp);

                    }
                }

            }

            return BadRequest("Username or Passoword incorrect!");
        }
    }
}
