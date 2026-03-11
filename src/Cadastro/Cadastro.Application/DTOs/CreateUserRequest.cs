namespace Cadastro.Application.DTOs;

/// <summary>
/// Request DTO for creating a new user.
/// </summary>
public sealed record CreateUserRequest(
    string Name,
    string Email);
