using AddressApi.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AddressApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress([FromBody] CreateAddressCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateAddress), new { id }, id);
    }
}
