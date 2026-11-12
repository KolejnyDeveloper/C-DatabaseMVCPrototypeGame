
using Osada.Models.Account;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Osada.Tests.Account;

public class LoginTests
{
    [Fact]
    public void Login_ShouldBeValid_WhenAllDataIsCorrect()
    {
        // Arrange
        var model = new LoginViewModel
        {
            Username = "Gracz123",
            Password = "Haslo123!"
        };

        // Act
        bool isValid = ValidateModel(model);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Login_ShouldBeInvalid_WhenUsernameIsEmpty()
    {
        // Arrange
        var model = new LoginViewModel
        {
            Username = "",
            Password = "Haslo123!"
        };

        // Act
        bool isValid = ValidateModel(model);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Login_ShouldBeInvalid_WhenPasswordIsEmpty()
    {
        // Arrange
        var model = new LoginViewModel
        {
            Username = "Gracz123",
            Password = ""
        };

        // Act
        bool isValid = ValidateModel(model);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Login_ShouldBeInvalid_WhenUsernameIsTooLong()
    {
        // Arrange
        var model = new LoginViewModel
        {
            Username = new string('A', 31),
            Password = "Haslo123!"
        };

        // Act
        bool isValid = ValidateModel(model);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Login_ShouldBeInvalid_WhenBothFieldsAreEmpty()
    {
        // Arrange
        var model = new LoginViewModel
        {
            Username = "",
            Password = ""
        };

        // Act
        bool isValid = ValidateModel(model);

        // Assert
        Assert.False(isValid);
    }

    private static bool ValidateModel(object model)
    {
        var results = new List<ValidationResult>();

        var context = new ValidationContext(model);

        return Validator.TryValidateObject(
            model,
            context,
            results,
            true);
    }
}
