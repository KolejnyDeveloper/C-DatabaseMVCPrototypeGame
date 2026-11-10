using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Osada.Models;

[Index("Username", Name = "UQ_Users_Username", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(30)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("NewPlayer")]
    public virtual ICollection<VillageOwnershipHistory> VillageOwnershipHistoryNewPlayers { get; set; } = new List<VillageOwnershipHistory>();

    [InverseProperty("PreviousPlayer")]
    public virtual ICollection<VillageOwnershipHistory> VillageOwnershipHistoryPreviousPlayers { get; set; } = new List<VillageOwnershipHistory>();

    [InverseProperty("Player")]
    public virtual ICollection<Village> Villages { get; set; } = new List<Village>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
