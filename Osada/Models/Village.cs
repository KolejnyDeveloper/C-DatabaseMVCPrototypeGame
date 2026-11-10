using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Osada.Models;

[Index("X", "Y", Name = "UQ_Villages_Coordinates", IsUnique = true)]
public partial class Village
{
    [Key]
    public int VillageId { get; set; }

    public int? PlayerId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public int X { get; set; }

    public int Y { get; set; }

    public int Points { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("AttackerVillage")]
    public virtual ICollection<Attack> AttackAttackerVillages { get; set; } = new List<Attack>();

    [InverseProperty("DefenderVillage")]
    public virtual ICollection<Attack> AttackDefenderVillages { get; set; } = new List<Attack>();

    [ForeignKey("PlayerId")]
    [InverseProperty("Villages")]
    public virtual User? Player { get; set; }

    [InverseProperty("Village")]
    public virtual ICollection<ResourceTransaction> ResourceTransactions { get; set; } = new List<ResourceTransaction>();

    [InverseProperty("Village")]
    public virtual ICollection<VillageBuilding> VillageBuildings { get; set; } = new List<VillageBuilding>();

    [InverseProperty("Village")]
    public virtual ICollection<VillageOwnershipHistory> VillageOwnershipHistories { get; set; } = new List<VillageOwnershipHistory>();

    [InverseProperty("Village")]
    public virtual VillageResource? VillageResource { get; set; }

    [InverseProperty("Village")]
    public virtual ICollection<VillageUnit> VillageUnits { get; set; } = new List<VillageUnit>();
}
