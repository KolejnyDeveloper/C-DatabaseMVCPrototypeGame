
using Osada.Models.ViewModels;
using Xunit;
using System.ComponentModel.DataAnnotations;

namespace Osada.Tests.Player;

public class RenameVillageTests
{
    [Fact]
    public void VillageName_ShouldBeValid_WhenNameIsCorrect()
    {
        // Arrange
        var model = new EditVillageViewModel
        {
            VillageId = 1,
            Name = "Nowa Osada"
        };

        // Act
        bool isValid = ValidateModel(model);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void VillageName_ShouldBeInvalid_WhenNameIsEmpty()
    {
        // Arrange
        var model = new EditVillageViewModel
        {
            VillageId = 1,
            Name = ""
        };

        // Act
        bool isValid = ValidateModel(model);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void VillageName_ShouldBeInvalid_WhenNameIsNull()
    {
        // Arrange
        var model = new EditVillageViewModel
        {
            VillageId = 1,
            Name = null!
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
