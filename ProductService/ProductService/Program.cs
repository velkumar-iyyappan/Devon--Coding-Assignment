using DataAccessLayer.ServiceExtension;
using ProductService.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRepositoryServices();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      builder =>
                      {
                          builder.AllowAnyOrigin().AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});


var app = builder.Build();

app.UseCors("MyAllowSpecificOrigins");


// Configure the HTTP request pipeline.

app.UseMiddleware<ApiKeyAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();


app.Run();
