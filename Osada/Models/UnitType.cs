using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Osada.Models;

[Index("Name", Name = "UQ_UnitTypes_Name", IsUnique = true)]
public partial class UnitType
{
    [Key]
    public int UnitTypeId { get; set; }

    [StringLength(40)]
    public string Name { get; set; } = null!;

    public int Attack { get; set; }

    public int Defense { get; set; }

    public int RecruitmentCostWood { get; set; }

    public int RecruitmentCostClay { get; set; }

    public int RecruitmentCostIron { get; set; }

    [InverseProperty("UnitType")]
    public virtual ICollection<VillageUnit> VillageUnits { get; set; } = new List<VillageUnit>();
}
