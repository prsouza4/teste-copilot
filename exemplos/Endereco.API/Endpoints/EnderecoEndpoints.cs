using Endereco.Application.Enderecos.Commands;
using Endereco.Application.Enderecos.Queries;
using MediatR;

namespace Endereco.API.Endpoints;

public static class EnderecoEndpoints
{
    public static void MapEnderecoEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/enderecos");

        group.MapPost("/", async (CreateEnderecoCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/enderecos/{result}", result);
        });

        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllEnderecosQuery());
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetEnderecoByIdQuery(id));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateEnderecoCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest();

            var result = await mediator.Send(command);
            return result ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteEnderecoCommand(id));
            return result ? Results.NoContent() : Results.NotFound();
        });
    }
}
