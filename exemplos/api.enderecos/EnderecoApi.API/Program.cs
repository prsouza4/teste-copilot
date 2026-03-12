using EnderecoApi.Application.Commands;
using EnderecoApi.Application.Handlers;
using EnderecoApi.Application.Validators;
using EnderecoApi.Domain.Interfaces;
using EnderecoApi.Infrastructure.Persistence;
using EnderecoApi.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EnderecoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddMediatR(typeof(CreateEnderecoCommandHandler));
builder.Services.AddValidatorsFromAssemblyContaining<CreateEnderecoCommandValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
