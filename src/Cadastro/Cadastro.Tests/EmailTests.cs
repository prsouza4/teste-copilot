using Cadastro.Domain.Exceptions;
using Cadastro.Domain.ValueObjects;
using FluentAssertions;

namespace Cadastro.Tests;

public class EmailTests
{
    [Theory]
    [InlineData("john@example.com")]
    [InlineData("john.doe@example.com")]
    [InlineData("john+tag@example.com")]
    [InlineData("john@sub.example.com")]
    [InlineData("john123@example.co.uk")]
    public void Create_WithValidEmail_ShouldSucceed(string emailValue)
    {
        // Act
        var email = Email.Create(emailValue);

        // Assert
        email.Value.Should().Be(emailValue.ToLowerInvariant());
    }

    [Fact]
    public void Create_WithUppercaseEmail_ShouldNormalize()
    {
        // Arrange
        var emailValue = "JOHN@EXAMPLE.COM";

        // Act
        var email = Email.Create(emailValue);

        // Assert
        email.Value.Should().Be("john@example.com");
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrim()
    {
        // Arrange
        var emailValue = "  john@example.com  ";

        // Act
        var email = Email.Create(emailValue);

        // Assert
        email.Value.Should().Be("john@example.com");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyOrNullEmail_ShouldThrowDomainException(string? emailValue)
    {
        // Act
        var act = () => Email.Create(emailValue!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Email cannot be empty.");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("@example.com")]
    [InlineData("invalid@example")]
    [InlineData("invalid@.com")]
    [InlineData("invalid@@example.com")]
    [InlineData("invalid @example.com")]
    public void Create_WithInvalidEmailFormat_ShouldThrowDomainException(string emailValue)
    {
        // Act
        var act = () => Email.Create(emailValue);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Email format is invalid.");
    }

    [Fact]
    public void Equals_WithSameValue_ShouldBeEqual()
    {
        // Arrange
        var email1 = Email.Create("john@example.com");
        var email2 = Email.Create("john@example.com");

        // Assert
        email1.Should().Be(email2);
        email1.Equals(email2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentCase_ShouldBeEqual()
    {
        // Arrange
        var email1 = Email.Create("john@example.com");
        var email2 = Email.Create("JOHN@EXAMPLE.COM");

        // Assert
        email1.Should().Be(email2);
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldNotBeEqual()
    {
        // Arrange
        var email1 = Email.Create("john@example.com");
        var email2 = Email.Create("jane@example.com");

        // Assert
        email1.Should().NotBe(email2);
        email1.Equals(email2).Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        var email = Email.Create("john@example.com");

        // Act
        string stringValue = email;

        // Assert
        stringValue.Should().Be("john@example.com");
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var email = Email.Create("john@example.com");

        // Act
        var result = email.ToString();

        // Assert
        result.Should().Be("john@example.com");
    }
}
