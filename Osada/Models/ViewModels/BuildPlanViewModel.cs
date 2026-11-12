
namespace Osada.Models.ViewModels;

public class BuildPlanViewModel
{
    public int VillageId { get; set; }

    public int WoodAmount { get; set; }
    public int ClayAmount { get; set; }
    public int IronAmount { get; set; }
    public int FoodAmount { get; set; }

    public List<BuildPlanItemViewModel> Buildings { get; set; } = new();
}

public class BuildPlanItemViewModel
{
    public int BuildingTypeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CurrentLevel { get; set; }

    public int NextLevel => CurrentLevel + 1;

    public int CostWood { get; set; }
    public int CostClay { get; set; }
    public int CostIron { get; set; }

    public bool IsBuilt => CurrentLevel > 0;
}
