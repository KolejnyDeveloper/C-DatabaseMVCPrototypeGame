using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Osada.Models;

[Index("VillageId", "BuildingTypeId", Name = "UQ_VillageBuildings", IsUnique = true)]
public partial class VillageBuilding
{
    [Key]
    public int VillageBuildingId { get; set; }

    public int VillageId { get; set; }

    public int BuildingTypeId { get; set; }

    public int Level { get; set; }

    [ForeignKey("BuildingTypeId")]
    [InverseProperty("VillageBuildings")]
    public virtual BuildingType BuildingType { get; set; } = null!;

    [ForeignKey("VillageId")]
    [InverseProperty("VillageBuildings")]
    public virtual Village Village { get; set; } = null!;
}
