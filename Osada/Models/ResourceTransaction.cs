using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Osada.Models;

public partial class ResourceTransaction
{
    [Key]
    public long TransactionId { get; set; }

    public int VillageId { get; set; }

    public int WoodChange { get; set; }

    public int OldWoodAmount { get; set; }

    public int NewWoodAmount { get; set; }

    public int ClayChange { get; set; }

    public int OldClayAmount { get; set; }

    public int NewClayAmount { get; set; }

    public int IronChange { get; set; }

    public int OldIronAmount { get; set; }

    public int NewIronAmount { get; set; }

    public int FoodChange { get; set; }

    public int OldFoodAmount { get; set; }

    public int NewFoodAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    [StringLength(100)]
    public string Description { get; set; } = null!;

    [ForeignKey("VillageId")]
    [InverseProperty("ResourceTransactions")]
    public virtual Village Village { get; set; } = null!;
}
