using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Osada.Models;

public partial class Attack
{
    [Key]
    public int AttackId { get; set; }

    public int AttackerVillageId { get; set; }

    public int DefenderVillageId { get; set; }

    public int AttackerPower { get; set; }

    public int DefenderPower { get; set; }

    public byte Result { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("AttackerVillageId")]
    [InverseProperty("AttackAttackerVillages")]
    public virtual Village AttackerVillage { get; set; } = null!;

    [ForeignKey("DefenderVillageId")]
    [InverseProperty("AttackDefenderVillages")]
    public virtual Village DefenderVillage { get; set; } = null!;
}
