using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using trackventory_backend.Dtos;
using trackventory_backend.Services;
using trackventory_backend.Services.Interfaces;

namespace trackventory_backend.Controllers
{
  [Route("api/auth")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtTokenManager _jwtTokenManager;
    private readonly ICustomCookieManager _cookieManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, JwtTokenManager jwtTokenManager, ICustomCookieManager cookieManager, ILogger<AuthController> logger) {
      _userManager = userManager;
      _roleManager = roleManager;
      _jwtTokenManager = jwtTokenManager;
      _cookieManager = cookieManager;
      _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto) {

      // Check matching passwords
      if (registerRequestDto.Password != registerRequestDto.ConfirmPassword) {
        return BadRequest("Password and Confirm Password does not match");
      }


      var newUser = new IdentityUser() {
        UserName = registerRequestDto.Email,
        Email = registerRequestDto.Email,
      };

      // Check if role exist
      foreach (string role in registerRequestDto.Roles!) {
        if (!await _roleManager.RoleExistsAsync(role)) {
          return BadRequest($"Role '{role}' does not exist.");
        }
      }

      // Register user
      var identityResult = await _userManager.CreateAsync(newUser, registerRequestDto.Password);

      if (!identityResult.Succeeded) {
        return BadRequest(identityResult.Errors);
      }


      return Ok("Registered succesffully");
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshAccessToken() {
      var userId = _cookieManager.GetCookie("UserId");
      var deviceId = _cookieManager.GetCookie("DeviceId");
      var RfToken = _cookieManager.GetCookie("RfToken");

      if (userId == null || deviceId == null || RfToken == null) return NotFound("Neccessary Cookies Not Found");

      // check valid token
      var isTokenValid = await _jwtTokenManager.ValidateRefreshTokenAsync(userId, deviceId, RfToken);

      if (!isTokenValid) return NotFound();

      // create new access token and refreshtoken and send back to the client
      // Search for user
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null) return NotFound("User Not Found");
      var roles = await _userManager.GetRolesAsync(user);

      var accessToken = _jwtTokenManager.GenerateJwtToken(user, roles.ToList());
      var refreshToken = _jwtTokenManager.GenerateRefreshToken();
      await _jwtTokenManager.AddOrUpdateRefreshTokenAsync(user.Id, deviceId, refreshToken, DateTime.Now.AddDays(15));

      int CookieExpiredTime = 10080; // 1 week
      _cookieManager.AddCookie("RfToken", refreshToken, CookieExpiredTime);

      var response = new {
        AccessToken = accessToken,
        //RefreshToken = refreshToken,
        User = new UserDto { Id = user.Id, Email = user.Email, UserName = user.UserName },
        Roles = roles
      };
      return Ok(response);
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest) {

      var user = await _userManager.FindByEmailAsync(loginRequest.Email);
      if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password)) {
        return Unauthorized("Invalid username or password");
      }

      var roles = await _userManager.GetRolesAsync(user);
      //int TTLInMinutes = 5;

      // Generate access token
      var accessToken = _jwtTokenManager.GenerateJwtToken(user, roles.ToList());
      // Generate refresh token
      var refreshToken = _jwtTokenManager.GenerateRefreshToken();

      // Store refreshtoken in db
      var deviceId = _cookieManager.GetCookie("DeviceId") ?? Guid.NewGuid().ToString();
      await _jwtTokenManager.AddOrUpdateRefreshTokenAsync(user.Id, deviceId, refreshToken, DateTime.Now.AddDays(15));

      // Store refresh token in cookie
      int CookieExpiredTime = 10080; // 1 week
      _cookieManager.AddCookie("RfToken", refreshToken, CookieExpiredTime);
      _cookieManager.AddCookie("DeviceId", deviceId, CookieExpiredTime);
      _cookieManager.AddCookie("UserId", user.Id, CookieExpiredTime);

      // Create a response
      var response = new {
        Message = "Login Successfully",
        AccessToken = accessToken,
        User = new UserDto { Id = user.Id, Email = user.Email, UserName = user.UserName },
        role = roles
      };

      return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {

      // revoke refresh token
      var userId = _cookieManager.GetCookie("UserId");
      var deviceId = _cookieManager.GetCookie("DeviceId");

      await _jwtTokenManager.RevokeRefreshTokenAsync(userId, deviceId);

      // clear cookies
      _cookieManager.DeleteCookie("RfToken");

      return Ok("Logout successfully");
    }


  }
}
