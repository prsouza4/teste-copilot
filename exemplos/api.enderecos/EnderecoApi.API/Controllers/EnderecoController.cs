using EnderecoApi.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnderecoApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnderecoController : ControllerBase
{
    private readonly IMediator _mediator;

    public EnderecoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEndereco([FromBody] CreateEnderecoCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetEnderecoById), new { id }, id);
    }

    [HttpGet("{id}")]
    public IActionResult GetEnderecoById(Guid id)
    {
        // Placeholder for actual implementation
        return Ok(new { id });
    }
}
