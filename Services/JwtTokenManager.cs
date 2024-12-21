using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using trackventory_backend.Data;
using trackventory_backend.Models;

namespace trackventory_backend.Services
{
  public class JwtTokenManager
  {
    private readonly IConfiguration _configuration;
    private readonly TrackventoryAuthDbContext _dbContext;

    public JwtTokenManager(IConfiguration configuration, TrackventoryAuthDbContext dbContext) {
      _configuration = configuration;
      _dbContext = dbContext;
    }
    public string GenerateJwtToken(IdentityUser user, List<string> roles, int TTLInMinute = 5) {
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

    public async Task AddOrUpdateRefreshTokenAsync(string userId, string deviceId, string rfToken, DateTime expiration) {
      var existingToken = await _dbContext.RefreshTokens
        .FirstOrDefaultAsync(rf => rf.UserId == userId && rf.DeviceId == deviceId && !rf.IsRevoked);

      if (existingToken != null) {
        // replace the old token with the new one
        existingToken.Token = rfToken;
        existingToken.Expiration = expiration;
        existingToken.DeviceId = deviceId;
      } else {
        // Add a new one
        _dbContext.RefreshTokens.Add(new RefreshTokens {
          UserId = userId,
          DeviceId = deviceId,
          Token = rfToken,
          Expiration = expiration
        });
      }
      await _dbContext.SaveChangesAsync();
    }


    public async Task<bool> ValidateRefreshTokenAsync(string userId, string deviceId, string token) {
      var refreshToken = await _dbContext.RefreshTokens
        .FirstOrDefaultAsync(rt =>
            rt.UserId == userId &&
            rt.DeviceId == deviceId &&
            rt.Token == token);

      return refreshToken != null && !refreshToken.IsRevoked && refreshToken.Expiration > DateTime.UtcNow;

    }

    public async Task RevokeRefreshTokenAsync(string userId, string deviceId) {
      var refreshToken = await _dbContext.RefreshTokens
        .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.DeviceId == deviceId && !rt.IsRevoked);

      if (refreshToken != null) {
        refreshToken.IsRevoked = true;
        await _dbContext.SaveChangesAsync();
      }
    }

    public async Task RevokeAllTokensAsync(string userId) {
      var tokens = await _dbContext.RefreshTokens
          .Where(rt => rt.UserId == userId && !rt.IsRevoked)
          .ToListAsync();

      foreach (var token in tokens) {
        token.IsRevoked = true;
      }

      await _dbContext.SaveChangesAsync();
    }

    public async Task RevokeDeviceTokenAsync(string userId, string deviceId) {
      var token = await _dbContext.RefreshTokens
          .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.DeviceId == deviceId);

      if (token != null) {
        token.IsRevoked = true;
        await _dbContext.SaveChangesAsync();
      }
    }

  }
}
