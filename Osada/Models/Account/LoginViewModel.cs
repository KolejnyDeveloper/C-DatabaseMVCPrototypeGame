using System.ComponentModel.DataAnnotations;

namespace Osada.Models.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Podaj nazwę użytkownika.")]
    [StringLength(30)]
    [Display(Name = "Nazwa użytkownika")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Podaj hasło.")]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    public string Password { get; set; } = string.Empty;
}