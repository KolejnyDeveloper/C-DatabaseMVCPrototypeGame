using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Osada.Models;

public partial class VillageResource
{
    [Key]
    public int VillageId { get; set; }

    public int WoodAmount { get; set; }

    public int ClayAmount { get; set; }

    public int IronAmount { get; set; }

    public int FoodAmount { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public int WoodProduction { get; set; }

    public int ClayProduction { get; set; }

    public int IronProduction { get; set; }

    public int FoodProduction { get; set; }

    [ForeignKey("VillageId")]
    [InverseProperty("VillageResource")]
    public virtual Village Village { get; set; } = null!;
}
