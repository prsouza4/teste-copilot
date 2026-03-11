using System.Text.RegularExpressions;
using Cadastro.Domain.Exceptions;

namespace Cadastro.Domain.ValueObjects;

/// <summary>
/// Value Object representing an email address.
/// </summary>
public sealed partial class Email : IEquatable<Email>
{
    private static readonly Regex EmailRegex = GenerateEmailRegex();

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new Email value object.
    /// </summary>
    /// <param name="value">The email string.</param>
    /// <returns>A validated Email instance.</returns>
    /// <exception cref="DomainException">Thrown when email is empty or has invalid format.</exception>
    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Email cannot be empty.");
        }

        var trimmedValue = value.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(trimmedValue))
        {
            throw new DomainException("Email format is invalid.");
        }

        return new Email(trimmedValue);
    }

    public bool Equals(Email? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Email other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(Email email) => email.Value;

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled)]
    private static partial Regex GenerateEmailRegex();
}
