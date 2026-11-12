
using System.ComponentModel.DataAnnotations;

namespace Osada.Models.ViewModels;

public class TransportResourcesViewModel
{
    [Required]
    public int SourceVillageId { get; set; }

    [Required]
    public int DestinationVillageId { get; set; }

    [Range(0, int.MaxValue)]
    public int Wood { get; set; }

    [Range(0, int.MaxValue)]
    public int Clay { get; set; }

    [Range(0, int.MaxValue)]
    public int Iron { get; set; }

    [Range(0, int.MaxValue)]
    public int Food { get; set; }
}
