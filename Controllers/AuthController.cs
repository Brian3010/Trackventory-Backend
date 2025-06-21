using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using trackventory_backend.Dtos;
using trackventory_backend.Helpers;
using trackventory_backend.Models;
using trackventory_backend.Services;
using trackventory_backend.Services.Interfaces;

namespace trackventory_backend.Controllers
{
  [Route("api/auth")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtTokenManager _jwtTokenManager;
    private readonly ICustomCookieManager _cookieManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, JwtTokenManager jwtTokenManager, ICustomCookieManager cookieManager, ILogger<AuthController> logger) {
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


      var newUser = new ApplicationUser() {
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

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshTokenRequestDto refreshTokenRequestDto) {

      _logger.LogInformation("refreshTokenRequestDto: {@refreshToken}", refreshTokenRequestDto);

      // IP Address
      var ipAddress = IpHelper.GetClientIp(HttpContext);

      //var userId = _cookieManager.GetCookie("UserId");
      var RfToken = _cookieManager.GetCookie("RfToken");


      if (RfToken == null) return BadRequest("Neccessary Cookies Not Found");

      // check valid token
      if (!await _jwtTokenManager.IsRefreshTokenExists(RfToken, refreshTokenRequestDto.DeviceId, ipAddress, refreshTokenRequestDto.UserId)) {
        return Unauthorized("Invalid Refresh Token");
      }

      var user = await _userManager.FindByIdAsync(refreshTokenRequestDto.UserId);
      if (user == null) return NotFound("User not exist");

      var roles = await _userManager.GetRolesAsync(user);

      var accessToken = _jwtTokenManager.GenerateJwtToken(user, roles.ToList());
      //var refreshToken = _jwtTokenManager.GenerateRefreshToken();
      //await _jwtTokenManager.AddOrUpdateRefreshTokenAsync(user.Id, deviceId, refreshToken, DateTime.Now.AddDays(15));

      //int CookieExpiredTime = 10080; // 1 week
      //_cookieManager.AddCookie("RfToken", refreshToken, CookieExpiredTime);

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
        return Unauthorized("Invalid email or password");
      }

      var roles = await _userManager.GetRolesAsync(user);
      //int TTLInMinutes = 5;

      // IP Address
      var ipAddress = IpHelper.GetClientIp(HttpContext);

      // Generate access token and refresh token
      var accessToken = _jwtTokenManager.GenerateJwtToken(user, roles.ToList());
      var refreshToken = _jwtTokenManager.GenerateRefreshToken();

      // Store refreshtoken in db
      await _jwtTokenManager.AddOrUpdateRefreshTokenAsync(user.Id, loginRequest.DeviceId, ipAddress, refreshToken, DateTime.Now.AddDays(15));

      // Store refresh token in cookie
      int CookieExpiredTime = 10080; // 1 week
      _cookieManager.AddCookie("RfToken", refreshToken, CookieExpiredTime);

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
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto logoutRequest) {

      /**
        * revoke refresh token
        * delete cookies
        */

      // Refresh token from cookie
      var refreshToken = Request.Cookies["RfToken"];
      if (string.IsNullOrEmpty(refreshToken)) return Unauthorized("Refresh token not found");

      // IP Address
      var ipAddress = IpHelper.GetClientIp(HttpContext);

      // Check if token exist before continure proceed
      if (!await _jwtTokenManager.IsRefreshTokenExists(refreshToken, logoutRequest.DeviceName, ipAddress!, logoutRequest.UserId)) {
        return Unauthorized("Refresh Token not exist.");
      }

      await _jwtTokenManager.RevokeRefreshTokenAsync(refreshToken, logoutRequest.DeviceName, ipAddress!, logoutRequest.UserId);

      // clear cookies
      Response.Cookies.Append("RfToken", "", new CookieOptions {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTime.UtcNow.AddDays(-1), // Set expiration in the past
      });

      return Ok("Logged out successfully.");
    }


    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto) {
      /*
       * Find the user email in the database -> wait for confirmation email
       * return not found user
       * 
       * replace password
       */
      var userEmail = resetPasswordRequestDto.UserEmail;

      var user = await _userManager.FindByEmailAsync(userEmail);

      if (user == null) {
        return NotFound("User Not Found");
      }

      // Can decode from Frontend
      var decodedToken = HttpUtility.UrlDecode(resetPasswordRequestDto.EmailToken);

      var res = await _userManager.ResetPasswordAsync(user, decodedToken, resetPasswordRequestDto.NewPassword);

      if (!res.Succeeded)
        return BadRequest(res.Errors);

      return Ok("Password has been reset successfully.");

    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] string email) {

      var user = await _userManager.FindByEmailAsync(email);
      if (user == null) return NotFound("User not found");

      var token = await _userManager.GeneratePasswordResetTokenAsync(user);

      var encodedToken = HttpUtility.UrlEncode(token);

      /* //TODO: Will need to send a link via email asking user to fill a form and hit POST reset-password
       * to reset password
       * 
       * For now, This API will send back the token to use for reseting password
       */

      return Ok(new { ResetToken = encodedToken });
      ;

    }


  }
}
