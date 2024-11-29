
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using trackventory_backend.Configurations;
using trackventory_backend.Data;
using trackventory_backend.Seed;
using trackventory_backend.Services;

namespace trackventory_backend
{
  public class Program
  {
    public static async Task Main(string[] args) {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();



      // Add Db Connection
      builder.Services.AddDbContext<TrackventoryAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TrackventoryAuthConnection")));

      // Add life-time services
      builder.Services.AddScoped<JwtTokenManager>();

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
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occurred while seeding the database.");
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
