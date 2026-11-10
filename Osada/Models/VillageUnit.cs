using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Osada.Models;

[PrimaryKey("VillageId", "UnitTypeId")]
public partial class VillageUnit
{
    [Key]
    public int VillageId { get; set; }

    [Key]
    public int UnitTypeId { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("UnitTypeId")]
    [InverseProperty("VillageUnits")]
    public virtual UnitType UnitType { get; set; } = null!;

    [ForeignKey("VillageId")]
    [InverseProperty("VillageUnits")]
    public virtual Village Village { get; set; } = null!;
}
