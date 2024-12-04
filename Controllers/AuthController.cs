using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using trackventory_backend.Dtos;
using trackventory_backend.Services;

namespace trackventory_backend.Controllers
{
  [Route("api/auth")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtTokenManager _jwtTokenManager;

    public AuthController(UserManager<IdentityUser> userManager, JwtTokenManager jwtTokenManager) {
      _userManager = userManager;
      _jwtTokenManager = jwtTokenManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest) {

      var user = await _userManager.FindByEmailAsync(loginRequest.Email);
      if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password)) {
        return Unauthorized("Invalid username or password");
      }

      var roles = await _userManager.GetRolesAsync(user);
      int TTLInMinutes = 5;

      // Generate access token
      var accessToken = _jwtTokenManager.GenerateJwtToken(user, roles.ToList(), TTLInMinutes);
      // Generate refresh token
      var refreshToken = _jwtTokenManager.GenerateRefreshToken();

      // Store refresh token in cookie
      int CookieExpiredTime = 10080; // 1 week

      // Create a response
      var response = new {
        Message = "Login Successfully",
        AccessToken = accessToken,
        User = new UserDto { Id = user.Id, Email = user.Email, UserName = user.UserName },
        role = roles
      };

      return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {

      /// Need to finish <see cref="CookieManager"/> first

      return Ok();
    }

  }
}
