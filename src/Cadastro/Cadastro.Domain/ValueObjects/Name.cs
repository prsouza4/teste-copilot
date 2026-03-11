using Cadastro.Domain.Exceptions;

namespace Cadastro.Domain.ValueObjects;

/// <summary>
/// Value Object representing a user's name.
/// </summary>
public sealed class Name : IEquatable<Name>
{
    public const int MaxLength = 100;

    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new Name value object.
    /// </summary>
    /// <param name="value">The name string.</param>
    /// <returns>A validated Name instance.</returns>
    /// <exception cref="DomainException">Thrown when name is empty or exceeds max length.</exception>
    public static Name Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Name cannot be empty.");
        }

        var trimmedValue = value.Trim();

        if (trimmedValue.Length > MaxLength)
        {
            throw new DomainException($"Name cannot exceed {MaxLength} characters.");
        }

        return new Name(trimmedValue);
    }

    public bool Equals(Name? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Name other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(Name name) => name.Value;
}
