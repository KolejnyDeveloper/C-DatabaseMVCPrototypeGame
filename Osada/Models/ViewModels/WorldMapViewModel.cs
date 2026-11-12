
namespace Osada.Models.ViewModels;

public class WorldMapViewModel
{
    public int CurrentVillageId { get; set; }

    public List<WorldMapVillageItemViewModel> Villages { get; set; } = new();
}

public class WorldMapVillageItemViewModel
{
    public int VillageId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string OwnerName { get; set; } = "Brak właściciela";

    public int X { get; set; }

    public int Y { get; set; }

    public int Points { get; set; }

    public bool IsOwnedByCurrentPlayer { get; set; }
}
