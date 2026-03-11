namespace Cadastro.Application.DTOs;

/// <summary>
/// Data transfer object for User.
/// </summary>
public sealed record UserDto(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    bool IsActive);
