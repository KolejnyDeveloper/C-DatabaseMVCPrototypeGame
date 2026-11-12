
namespace Osada.Models.ViewModels;

public class BarracksViewModel
{
    public int VillageId { get; set; }

    public List<VillageUnitItemViewModel> OwnedUnits { get; set; } = new();

    public List<RecruitableUnitViewModel> RecruitableUnits { get; set; } = new();

    public int WoodAmount { get; set; }
    public int ClayAmount { get; set; }
    public int IronAmount { get; set; }
    public int FoodAmount { get; set; }
}

public class VillageUnitItemViewModel
{
    public int UnitTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
}

public class RecruitableUnitViewModel
{
    public int UnitTypeId { get; set; }
    public string Name { get; set; } = string.Empty;

    public int Attack { get; set; }
    public int Defense { get; set; }

    public int RecruitmentCostWood { get; set; }
    public int RecruitmentCostClay { get; set; }
    public int RecruitmentCostIron { get; set; }
}
