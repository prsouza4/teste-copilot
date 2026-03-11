using Cadastro.Domain.Exceptions;
using Cadastro.Domain.ValueObjects;
using FluentAssertions;

namespace Cadastro.Tests;

public class NameTests
{
    [Fact]
    public void Create_WithValidName_ShouldSucceed()
    {
        // Arrange
        var nameValue = "John Doe";

        // Act
        var name = Name.Create(nameValue);

        // Assert
        name.Value.Should().Be(nameValue);
    }

    [Fact]
    public void Create_WithNameContainingWhitespace_ShouldTrim()
    {
        // Arrange
        var nameValue = "  John Doe  ";

        // Act
        var name = Name.Create(nameValue);

        // Assert
        name.Value.Should().Be("John Doe");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyOrNullName_ShouldThrowDomainException(string? nameValue)
    {
        // Act
        var act = () => Name.Create(nameValue!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Name cannot be empty.");
    }

    [Fact]
    public void Create_WithNameExceedingMaxLength_ShouldThrowDomainException()
    {
        // Arrange
        var nameValue = new string('A', Name.MaxLength + 1);

        // Act
        var act = () => Name.Create(nameValue);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage($"Name cannot exceed {Name.MaxLength} characters.");
    }

    [Fact]
    public void Create_WithNameAtMaxLength_ShouldSucceed()
    {
        // Arrange
        var nameValue = new string('A', Name.MaxLength);

        // Act
        var name = Name.Create(nameValue);

        // Assert
        name.Value.Should().HaveLength(Name.MaxLength);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldBeEqual()
    {
        // Arrange
        var name1 = Name.Create("John Doe");
        var name2 = Name.Create("John Doe");

        // Assert
        name1.Should().Be(name2);
        name1.Equals(name2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldNotBeEqual()
    {
        // Arrange
        var name1 = Name.Create("John Doe");
        var name2 = Name.Create("Jane Doe");

        // Assert
        name1.Should().NotBe(name2);
        name1.Equals(name2).Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        var name = Name.Create("John Doe");

        // Act
        string stringValue = name;

        // Assert
        stringValue.Should().Be("John Doe");
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var name = Name.Create("John Doe");

        // Act
        var result = name.ToString();

        // Assert
        result.Should().Be("John Doe");
    }
}
