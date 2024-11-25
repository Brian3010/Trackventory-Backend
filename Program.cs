
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
        //.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Trackventory")
        .AddEntityFrameworkStores<TrackventoryAuthDbContext>()
        .AddDefaultTokenProviders();


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

      app.UseHttpsRedirection();

      app.UseAuthorization();
      app.UseAuthentication();


      app.MapControllers();

      app.Run();
    }
  }
}
