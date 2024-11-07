using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Identity;
public class RegisterDto
{
    [Required(ErrorMessage = "Email is Required!")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "DisplayName is Required!")]
    public string DisplayName { get; set; }

    [Required(ErrorMessage = "PhoneNumber is Required!")]
    [Phone]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Password is Required!")]
    public string Password { get; set; }
}
