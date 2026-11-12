
using System.ComponentModel.DataAnnotations;

namespace Osada.Models.ViewModels;

public class CreateVillageViewModel
{
    [Required(ErrorMessage = "Podaj nazwę wioski.")]
    [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Nazwa musi mieć od 3 do 50 znaków.")]
    public string Name { get; set; } = string.Empty;
}
