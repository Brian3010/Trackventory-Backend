
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using trackventory_backend.Configurations;
using trackventory_backend.Data;
using trackventory_backend.Repositories;
using trackventory_backend.Repositories.Interfaces;
using trackventory_backend.Seed;
using trackventory_backend.Services;
using trackventory_backend.Services.Interfaces;

namespace trackventory_backend
{
  public class Program
  {
    public static async Task Main(string[] args) {
      var builder = WebApplication.CreateBuilder(args);

      // Add Serilog
      var logger = new LoggerConfiguration().WriteTo
        .Console(outputTemplate:
        "{NewLine}[{Timestamp:HH:mm}] {Message:lj}{NewLine}{Exception}")
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) // Suppress Microsoft logs below Warning
        .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning) // Suppress System logs below Warning
        .CreateLogger();

      builder.Logging.ClearProviders();
      builder.Logging.AddSerilog(logger);
      logger.Information("Serilog starting");
      logger.Information($"Total services: {builder.Services.Count}");


      // Add services to the container.
      builder.Services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen(options => {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "RestroFlow APIs", Version = "V1" });
        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme {
          Name = "Authorization",
          Description = "Enter 'Bearer' following by space and JWT.",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "bearer",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
          {
            new OpenApiSecurityScheme {
              Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme },
              Scheme ="Oauth2",
              Name = JwtBearerDefaults.AuthenticationScheme,
              In = ParameterLocation.Header
            },
             new List<string>()
          }
        });
      });



      // Add Db Connection
      builder.Services.AddDbContext<TrackventoryAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TrackventoryAuthConnection")));

      builder.Services.AddDbContext<TrackventoryDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TrackventoryConnection")));

      // Add life-time services
      builder.Services.AddHttpContextAccessor();
      builder.Services.AddScoped<JwtTokenManager>();
      builder.Services.AddScoped<ICustomCookieManager, CookieManager>();
      builder.Services.AddScoped<IInventoryRepository, SQLInventoryRepository>();
      builder.Services.AddScoped<IExcelConverter, ExcelConverter>();
      builder.Services.AddScoped<IEmailService, EmailService>();

      // Add Identity system to the ASP.NET Core service container
      builder.Services.AddIdentityCore<IdentityUser>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<TrackventoryAuthDbContext>();


      // Configure identity options
      builder.Services.Configure<IdentityOptions>(options => {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 6;
      });

      // Add JWT
      builder.Services.ConfigureOptions<JwtBearerConfigurationOptions>().AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();


      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      // Invoke the seed data -> run on the first build to seed users and roles
      using (var scope = app.Services.CreateScope()) {
        var services = scope.ServiceProvider;
        try {
          await SeedAuthData.InitializeAsync(services);
        } catch (Exception ex) {
          //var logger = services.GetRequiredService<ILogger<Program>>();
          logger.Information(ex, "An error occurred while seeding the database.");
        }
      }

      app.UseHttpsRedirection();

      // Add middleware
      app.UseAuthentication(); // Enable authentication middleware
      app.UseAuthorization();  // Enable authorization middleware


      app.MapControllers();

      app.Run();
    }
  }
}
