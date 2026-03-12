using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Usuario.Application.Users.Commands.CreateUser;

namespace Usuario.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetUserById), new { id = result.Value }, null);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetUserById(Guid id)
        {
            // Placeholder for GetUserById implementation
            return Ok();
        }
    }
}
