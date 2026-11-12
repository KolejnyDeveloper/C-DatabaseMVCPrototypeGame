
using System.ComponentModel.DataAnnotations;

namespace Osada.Models.ViewModels;

public class EditVillageViewModel
{
    public int VillageId { get; set; }

    [Required(ErrorMessage = "Podaj nazwę wioski.")]
    public string Name { get; set; } = string.Empty;
}
