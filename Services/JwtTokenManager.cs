using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace trackventory_backend.Services
{
  public class JwtTokenManager
  {
    private readonly IConfiguration _configuration;

    public JwtTokenManager(IConfiguration configuration) {
      _configuration = configuration;
    }
    public string GenerateJwtToken(IdentityUser user, List<string> roles, int TTLInMinute) {
      var jwtSettings = _configuration.GetSection("JwtSettings");

      var claims = new List<Claim>() {
        new Claim(ClaimTypes.Email,user.Email),
        new Claim(ClaimTypes.Name, user.UserName),
      };

      foreach (var role in roles) {
        claims.Add(new Claim(ClaimTypes.Role, role));
      }

      // Generate a symmetric security key from the secret key
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
      var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      // Create the JWT token with issuer, audience, claims, expiration time, and signing credentials
      var token = new JwtSecurityToken(
        issuer: jwtSettings["Issuer"],
        audience: jwtSettings["Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(TTLInMinute),
        signingCredentials: credential
        );

      // Serialize the token to a string and return it
      return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public string GenerateRefreshToken() {
      const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
      StringBuilder result = new StringBuilder();
      Random random = new Random();

      for (int i = 0; i < 64; i++) {
        result.Append(validChars[random.Next(validChars.Length)]);
      }

      return result.ToString();
    }

  }
}
