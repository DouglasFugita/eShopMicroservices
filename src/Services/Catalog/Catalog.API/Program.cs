using Catalog.API.Configuration;
using Catalog.API.Repositories;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.RegisterServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


