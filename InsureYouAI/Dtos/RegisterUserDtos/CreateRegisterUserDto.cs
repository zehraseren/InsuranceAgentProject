using System.ComponentModel.DataAnnotations;

namespace InsureYouAI.Dtos.RegisterUserDtos;

public class CreateRegisterUserDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    [Required(ErrorMessage ="Kullanım şartlarını kabul etmeniz gerekmektedir!")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Kullanım şartlarını kabul etmeniz gerekmektedir!")]
    public bool AcceptTerms { get; set; }
}
