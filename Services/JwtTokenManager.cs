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

    public async Task<bool> IsRefreshTokenExists(string refreshToken, string deviceId, string deviceIpAddress, string userId) {

      deviceId ??= "Unknown Device";

      var foundToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rf => rf.UserId == userId && rf.DeviceName == deviceId && rf.DeviceIpAddress == deviceIpAddress && !rf.IsRevoked);

      return foundToken != null;
    }

    public async Task AddOrUpdateRefreshTokenAsync(string userId, string deviceId, string IpAddress, string rfToken, DateTime expiration) {

      // Check if token exists with deviceName, deviceIpAddress, userId
      if (await IsRefreshTokenExists(rfToken, deviceId, IpAddress, userId)) {
        // Update
        await UpdateRefreshToken(rfToken, deviceId, IpAddress, userId);
      } else {
        // Store
        await StoreRefreshToken(rfToken, deviceId, IpAddress, userId);
      }
    }

    private async Task StoreRefreshToken(string refreshToken, string deviceName, string deviceIpAddress, string userId) {

      _dbContext.RefreshTokens.Add(new RefreshTokens {
        Id = new Guid(),
        UserId = userId,
        Token = refreshToken,
        CreatedAt = DateTime.UtcNow,
        ExpiredAt = DateTime.UtcNow.AddDays(7),
        IsRevoked = false,
        DeviceName = deviceName ??= "Unknown Device",
        DeviceIpAddress = deviceIpAddress ?? "localhost",
      });


      await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateRefreshToken(string refreshToken, string deviceName, string deviceIpAddress, string userId) {
      var rfToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.DeviceName == deviceName && r.DeviceIpAddress == deviceIpAddress && r.UserId == userId && !r.IsRevoked);

      if (rfToken != null) {
        rfToken.Token = refreshToken;
        rfToken.IsRevoked = false;
      }
      await _dbContext.SaveChangesAsync();
    }


    public async Task RevokeRefreshTokenAsync(string refreshToken, string deviceName, string deviceIpAddress, string userId) {
      // Find the token
      var rfToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.DeviceName == deviceName && r.DeviceIpAddress == deviceIpAddress && r.UserId == userId && r.Token == refreshToken && !r.IsRevoked);
      //_logger.LogInformation("rfToken: {rfToken}", rfToken);


      if (rfToken != null) {
        rfToken.IsRevoked = true;
        rfToken.RevokedAt = DateTime.Now;
      }

      await _dbContext.SaveChangesAsync();
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

  }
}
