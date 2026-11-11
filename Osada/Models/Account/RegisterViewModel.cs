using System.ComponentModel.DataAnnotations;

namespace Osada.Models.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Podaj nazwę użytkownika.")]
    [StringLength(30, MinimumLength = 3,
        ErrorMessage = "Nazwa musi mieć od 3 do 30 znaków.")]
    [Display(Name = "Nazwa użytkownika")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Podaj hasło.")]
    [StringLength(100, MinimumLength = 8,
        ErrorMessage = "Hasło musi mieć od 8 do 100 znaków.")]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Powtórz hasło.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password),
        ErrorMessage = "Podane hasła nie są identyczne.")]
    [Display(Name = "Powtórz hasło")]
    public string ConfirmPassword { get; set; } = string.Empty;
}