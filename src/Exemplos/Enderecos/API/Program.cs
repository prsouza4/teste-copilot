using Exemplos.Enderecos.Application.Handlers;
using Exemplos.Enderecos.Domain.Interfaces;
using Exemplos.Enderecos.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EnderecoDbContext>(options =>
    options.UseInMemoryDatabase("EnderecoDb"));

builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddMediatR(typeof(CreateEnderecoCommandHandler).Assembly);

builder.Services.AddControllers();
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
