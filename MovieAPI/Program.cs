using Microsoft.EntityFrameworkCore;
using MovieAPI.Endpoints;
using MovieAPI.Persistence;
using MovieAPI.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MovieDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});
builder.Services.AddTransient<IMovieService, MovieService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    await using var scope = app.Services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.MapGet("/", () => "Hello World!")
   .Produces(200, typeof(string));
app.MapMovieEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
