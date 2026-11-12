
//using Osada.Models.ViewModels;
//using Xunit;

//namespace Osada.Tests;

//public class CreateVillageTests
//{
//    [Fact]
//    public void VillageName_ShouldBeInvalid_WhenItIsTooShort()
//    {
//        var model = new CreateVillageViewModel
//        {
//            Name = "AB"
//        };

//        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

//        var validationContext =
//            new System.ComponentModel.DataAnnotations.ValidationContext(model);

//        bool isValid =
//            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
//                model,
//                validationContext,
//                validationResults,
//                true);

//        Assert.False(isValid);
//    }

//    [Fact]
//    public void VillageName_ShouldBeValid_WhenNameIsCorrect()
//    {
//        var model = new CreateVillageViewModel
//        {
//            Name = "Nowa Osada"
//        };

//        var validationResults =
//            new List<System.ComponentModel.DataAnnotations.ValidationResult>();

//        var validationContext =
//            new System.ComponentModel.DataAnnotations.ValidationContext(model);

//        bool isValid =
//            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
//                model,
//                validationContext,
//                validationResults,
//                true);

//        Assert.True(isValid);
//    }

//    [Fact]
//    public void VillageName_ShouldBeInvalid_WhenNameIsEmpty()
//    {
//        var model = new CreateVillageViewModel
//        {
//            Name = ""
//        };

//        var validationResults =
//            new List<System.ComponentModel.DataAnnotations.ValidationResult>();

//        var validationContext =
//            new System.ComponentModel.DataAnnotations.ValidationContext(model);

//        bool isValid =
//            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
//                model,
//                validationContext,
//                validationResults,
//                true);

//        Assert.False(isValid);
//    }

//    [Fact]
//    public void VillageName_ShouldBeInvalid_WhenNameIsTooLong()
//    {
//        var model = new CreateVillageViewModel
//        {
//            Name = "LipinkiŁużyckieŁączna43ZarazKołoPocztyObjazdTutajZgłaszamWybuchStyrteKtośNamPodpaliłStyrteCoPodbaliłNiewiemKto"
//        };

//        var validationResults =
//            new List<System.ComponentModel.DataAnnotations.ValidationResult>();

//        var validationContext =
//            new System.ComponentModel.DataAnnotations.ValidationContext(model);

//        bool isValid =
//            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
//                model,
//                validationContext,
//                validationResults,
//                true);

//        Assert.False(isValid);
//    }

//}
