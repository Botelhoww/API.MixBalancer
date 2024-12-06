using Microsoft.EntityFrameworkCore;
using MixBalancer.Infrastructure.Context;
using MixBalancer.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPersistence(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<MixBalancerContext>();
context.Database.Migrate();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();