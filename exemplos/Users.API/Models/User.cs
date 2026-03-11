namespace Users.API.Models;

/// <summary>
/// Represents a user entity in the system.
/// </summary>
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for creating a new user.
/// </summary>
public record CreateUserRequest(string Name, string Email);

/// <summary>
/// DTO for updating an existing user.
/// </summary>
public record UpdateUserRequest(string Name, string Email);
