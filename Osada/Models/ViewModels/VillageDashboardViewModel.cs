
namespace Osada.Models.ViewModels;

public class VillageDashboardViewModel
{
    public int VillageId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int X { get; set; }

    public int Y { get; set; }

    public int Points { get; set; }

    public string Section { get; set; } = "TownHall";

    public bool HasBarracks { get; set; }

    public int WoodAmount { get; set; }
    public int ClayAmount { get; set; }
    public int IronAmount { get; set; }
    public int FoodAmount { get; set; }

    public int WoodProduction { get; set; }
    public int ClayProduction { get; set; }
    public int IronProduction { get; set; }
    public int FoodProduction { get; set; }

    public List<BuildingItemViewModel> Buildings { get; set; } = new();
    public BarracksViewModel Barracks { get; set; } = new();
    public BuildPlanViewModel BuildPlan { get; set; } = new();

    public WorldMapViewModel WorldMap { get; set; } = new();
}

public class BuildingItemViewModel
{
    public string Name { get; set; } = string.Empty;

    public int Level { get; set; }
}
