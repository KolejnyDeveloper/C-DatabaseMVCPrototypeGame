
using Osada.Models;
using Xunit;

namespace Osada.Tests.Player;

public class VillageUnitTests
{
    [Fact]
    public void VillageUnit_ShouldStoreCorrectQuantity()
    {
        // Arrange
        var unit = new VillageUnit
        {
            VillageId = 1,
            UnitTypeId = 2,
            Quantity = 10
        };

        // Assert
        Assert.Equal(1, unit.VillageId);
        Assert.Equal(2, unit.UnitTypeId);
        Assert.Equal(10, unit.Quantity);
    }

    [Fact]
    public void UnitType_ShouldStoreCorrectRecruitmentCosts()
    {
        // Arrange
        var unitType = new UnitType
        {
            Name = "Miecznik",
            Attack = 20,
            Defense = 15,
            RecruitmentCostWood = 50,
            RecruitmentCostClay = 30,
            RecruitmentCostIron = 20
        };

        // Assert
        Assert.Equal(50, unitType.RecruitmentCostWood);
        Assert.Equal(30, unitType.RecruitmentCostClay);
        Assert.Equal(20, unitType.RecruitmentCostIron);
    }
}
