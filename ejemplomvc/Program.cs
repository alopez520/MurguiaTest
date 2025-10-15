using ejemplomvc.Middleware;
using ejemplomvc.Models;
using ejemplomvc.Repositories;
using ejemplomvc.Repositories.Interfaces;
using ejemplomvc.Services;
using ejemplomvc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions => {
        sqlOptions.CommandTimeout(5);
    })
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information)
 );

builder.Services.AddScoped<ITareaRepository, TareaRepositoryCsv>();
builder.Services.AddScoped<ITareaService, TareaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
        ctx.Context.Response.Headers.Append("Pragma", "no-cache");
        ctx.Context.Response.Headers.Append("Expires", "0");
    }
});

app.UseApiExceptionHandler();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.Run();