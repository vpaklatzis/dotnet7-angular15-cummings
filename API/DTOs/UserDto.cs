using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class UserDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string JwToken { get; set; }
}