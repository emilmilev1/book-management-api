using BookManagementApi.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

// seed db
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var libraryDbContext = services.GetRequiredService<LibraryDbContext>();
    SeedData.CreateLibrary(libraryDbContext);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
