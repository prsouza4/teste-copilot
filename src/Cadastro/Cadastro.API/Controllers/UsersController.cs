using Cadastro.Application.Commands;
using Cadastro.Application.DTOs;
using Cadastro.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cadastro.API.Controllers;

/// <summary>
/// Controller for user management operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ISender sender, ILogger<UsersController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the created user.</returns>
    /// <response code="201">User created successfully.</response>
    /// <response code="400">Invalid request data or domain rule violation.</response>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating user with email: {Email}", request.Email);

        var command = new CreateUserCommand(request.Name, request.Email);
        var userId = await _sender.Send(command, cancellationToken);

        _logger.LogInformation("User created with ID: {UserId}", userId);

        return CreatedAtAction(
            nameof(GetUserById),
            new { id = userId },
            new { id = userId });
    }

    /// <summary>
    /// Gets a user by their ID.
    /// </summary>
    /// <param name="id">The user's ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user data.</returns>
    /// <response code="200">User found.</response>
    /// <response code="404">User not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting user with ID: {UserId}", id);

        var query = new GetUserByIdQuery(id);
        var user = await _sender.Send(query, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", id);
            return NotFound();
        }

        return Ok(user);
    }

    /// <summary>
    /// Lists users with pagination.
    /// </summary>
    /// <param name="page">Page number (1-based). Default: 1.</param>
    /// <param name="pageSize">Number of items per page (1-100). Default: 10.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of users.</returns>
    /// <response code="200">Users retrieved successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Listing users. Page: {Page}, PageSize: {PageSize}", page, pageSize);

        var query = new ListUsersQuery(page, pageSize);
        var result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }
}
