using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Osada.Models;

[Table("VillageOwnershipHistory")]
public partial class VillageOwnershipHistory
{
    [Key]
    public long HistoryId { get; set; }

    public int VillageId { get; set; }

    public int? PreviousPlayerId { get; set; }

    public int? NewPlayerId { get; set; }

    public DateTime ChangedAt { get; set; }

    [ForeignKey("NewPlayerId")]
    [InverseProperty("VillageOwnershipHistoryNewPlayers")]
    public virtual User? NewPlayer { get; set; }

    [ForeignKey("PreviousPlayerId")]
    [InverseProperty("VillageOwnershipHistoryPreviousPlayers")]
    public virtual User? PreviousPlayer { get; set; }

    [ForeignKey("VillageId")]
    [InverseProperty("VillageOwnershipHistories")]
    public virtual Village Village { get; set; } = null!;
}
