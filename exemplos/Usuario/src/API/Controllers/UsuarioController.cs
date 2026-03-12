using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Usuario.Application.Commands;

namespace Usuario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuarioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(ObterUsuarioPorId), new { id = result.Value }, null);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterUsuarioPorId(Guid id)
    {
        // Implementar consulta
        return Ok();
    }
}
