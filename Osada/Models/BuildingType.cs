using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Osada.Models;

[Index("Name", Name = "UQ_BuildingTypes_Name", IsUnique = true)]
public partial class BuildingType
{
    [Key]
    public int BuildingTypeId { get; set; }

    [StringLength(40)]
    public string Name { get; set; } = null!;

    public int BaseCostWood { get; set; }

    public int BaseCostClay { get; set; }

    public int BaseCostIron { get; set; }

    public int WoodProductionBonus { get; set; }

    public int ClayProductionBonus { get; set; }

    public int IronProductionBonus { get; set; }

    public int FoodProductionBonus { get; set; }

    [InverseProperty("BuildingType")]
    public virtual ICollection<VillageBuilding> VillageBuildings { get; set; } = new List<VillageBuilding>();
}
