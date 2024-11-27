
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using trackventory_backend.Data;

namespace trackventory_backend
{
  public class Program
  {
    public static void Main(string[] args) {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();



      // Add Db Connection
      builder.Services.AddDbContext<TrackventoryAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TrackventoryAuthConnection")));

      // Add Identity system to the ASP.NET Core service container
      builder.Services.AddIdentityCore<IdentityUser>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<TrackventoryAuthDbContext>();


      // Configure identity options
      builder.Services.Configure<IdentityOptions>(options => {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 6;
      });

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      //// Invoke the seed data
      //using (var scope = app.Services.CreateScope()) {
      //  var services = scope.ServiceProvider;
      //  try {
      //    await SeedAuthData.InitializeAsync(services);
      //  } catch (Exception ex) {
      //    var logger = services.GetRequiredService<ILogger<Program>>();
      //    logger.LogError(ex, "An error occurred while seeding the database.");
      //  }
      //}

      app.UseHttpsRedirection();

      app.UseAuthorization();
      //app.UseAuthentication();


      app.MapControllers();

      app.Run();
    }
  }
}
