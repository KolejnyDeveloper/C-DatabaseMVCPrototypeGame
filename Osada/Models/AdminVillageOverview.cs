namespace Osada.Models;

public class AdminVillageOverview
{
    public int VillageId { get; set; }
    public string VillageName { get; set; } = string.Empty;

    public int X { get; set; }
    public int Y { get; set; }

    public int Points { get; set; }

    public int? OwnerId { get; set; }
    public string? OwnerUsername { get; set; }
    public bool? OwnerIsActive { get; set; }

    public int? WoodAmount { get; set; }
    public int? ClayAmount { get; set; }
    public int? IronAmount { get; set; }
    public int? FoodAmount { get; set; }

    public int? WoodProduction { get; set; }
    public int? ClayProduction { get; set; }
    public int? IronProduction { get; set; }
    public int? FoodProduction { get; set; }

    public int BuildingCount { get; set; }
    public int TotalBuildingLevels { get; set; }

    public int TotalUnits { get; set; }

    public int TotalAttack { get; set; }
    public int TotalDefense { get; set; }

    public int AttacksLaunched { get; set; }
    public int AttacksReceived { get; set; }
}