using Cadastro.Domain.Entities;
using Cadastro.Domain.Events;
using Cadastro.Domain.Exceptions;
using FluentAssertions;

namespace Cadastro.Tests;

public class UserTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var name = "John Doe";
        var email = "john@example.com";

        // Act
        var user = User.Create(name, email);

        // Assert
        user.Id.Should().NotBeEmpty();
        user.Name.Value.Should().Be(name);
        user.Email.Value.Should().Be(email.ToLowerInvariant());
        user.IsActive.Should().BeTrue();
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_WithValidData_ShouldRaiseUserCreatedEvent()
    {
        // Arrange
        var name = "John Doe";
        var email = "john@example.com";

        // Act
        var user = User.Create(name, email);

        // Assert
        user.DomainEvents.Should().ContainSingle();
        var domainEvent = user.DomainEvents.First();
        domainEvent.Should().BeOfType<UserCreatedEvent>();

        var userCreatedEvent = (UserCreatedEvent)domainEvent;
        userCreatedEvent.UserId.Should().Be(user.Id);
        userCreatedEvent.Name.Should().Be(name);
        userCreatedEvent.Email.Should().Be(email.ToLowerInvariant());
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowDomainException()
    {
        // Arrange
        var name = "";
        var email = "john@example.com";

        // Act
        var act = () => User.Create(name, email);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Name cannot be empty.");
    }

    [Fact]
    public void Create_WithInvalidEmail_ShouldThrowDomainException()
    {
        // Arrange
        var name = "John Doe";
        var email = "invalid-email";

        // Act
        var act = () => User.Create(name, email);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Email format is invalid.");
    }

    [Fact]
    public void Deactivate_WhenActive_ShouldDeactivate()
    {
        // Arrange
        var user = User.Create("John Doe", "john@example.com");

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Deactivate_WhenAlreadyInactive_ShouldThrowDomainException()
    {
        // Arrange
        var user = User.Create("John Doe", "john@example.com");
        user.Deactivate();

        // Act
        var act = () => user.Deactivate();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("User is already deactivated.");
    }

    [Fact]
    public void Activate_WhenInactive_ShouldActivate()
    {
        // Arrange
        var user = User.Create("John Doe", "john@example.com");
        user.Deactivate();

        // Act
        user.Activate();

        // Assert
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Activate_WhenAlreadyActive_ShouldThrowDomainException()
    {
        // Arrange
        var user = User.Create("John Doe", "john@example.com");

        // Act
        var act = () => user.Activate();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("User is already active.");
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var user = User.Create("John Doe", "john@example.com");
        var newName = "Jane Doe";

        // Act
        user.UpdateName(newName);

        // Assert
        user.Name.Value.Should().Be(newName);
    }

    [Fact]
    public void UpdateName_WithEmptyName_ShouldThrowDomainException()
    {
        // Arrange
        var user = User.Create("John Doe", "john@example.com");

        // Act
        var act = () => user.UpdateName("");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Name cannot be empty.");
    }

    [Fact]
    public void ClearDomainEvents_ShouldClearAllEvents()
    {
        // Arrange
        var user = User.Create("John Doe", "john@example.com");
        user.DomainEvents.Should().NotBeEmpty();

        // Act
        user.ClearDomainEvents();

        // Assert
        user.DomainEvents.Should().BeEmpty();
    }
}
