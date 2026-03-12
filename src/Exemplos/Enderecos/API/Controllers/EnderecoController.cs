using Exemplos.Enderecos.Application.Commands;
using Exemplos.Enderecos.Application.DTOs;
using Exemplos.Enderecos.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Exemplos.Enderecos.API.Controllers;

[ApiController]
[Route("api/enderecos")]
public class EnderecoController : ControllerBase
{
    private readonly IMediator _mediator;

    public EnderecoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllEnderecosQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetEnderecoByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEnderecoCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEnderecoCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID do endereço não corresponde ao ID fornecido.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteEnderecoCommand(id));
        return NoContent();
    }
}
