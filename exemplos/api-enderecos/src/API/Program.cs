using ApiEnderecos.Application.Commands;
using ApiEnderecos.Application.Queries;
using ApiEnderecos.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AddressDbContext>(options =>
    options.UseInMemoryDatabase("AddressDb"));

builder.Services.AddMediatR(typeof(CreateAddressCommand).Assembly);
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

var app = builder.Build();

app.MapPost("/addresses", async (CreateAddressCommand command, IMediator mediator) =>
{
    var id = await mediator.Send(command);
    return Results.Created($"/addresses/{id}", id);
});

app.MapPut("/addresses/{id:guid}", async (Guid id, UpdateAddressCommand command, IMediator mediator) =>
{
    await mediator.Send(command with { Id = id });
    return Results.NoContent();
});

app.MapDelete("/addresses/{id:guid}", async (Guid id, IMediator mediator) =>
{
    await mediator.Send(new DeleteAddressCommand(id));
    return Results.NoContent();
});

app.MapGet("/addresses/{id:guid}", async (Guid id, IMediator mediator) =>
{
    var address = await mediator.Send(new GetAddressByIdQuery(id));
    return Results.Ok(address);
});

app.MapGet("/addresses", async (IMediator mediator) =>
{
    var addresses = await mediator.Send(new GetAllAddressesQuery());
    return Results.Ok(addresses);
});

app.Run();
