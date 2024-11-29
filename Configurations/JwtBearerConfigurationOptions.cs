using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace trackventory_backend.Configurations
{
  public class JwtBearerConfigurationOptions : IConfigureNamedOptions<JwtBearerOptions>
  {
    private readonly IConfiguration _configuration;

    public JwtBearerConfigurationOptions(IConfiguration configuration) {
      _configuration = configuration;
    }


    public void Configure(string? name, JwtBearerOptions options) {
      var jwtSettings = _configuration.GetSection("JwtSettings");

      options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
      };
    }

    public void Configure(JwtBearerOptions options) => Configure(Options.DefaultName, options);
  }
}
