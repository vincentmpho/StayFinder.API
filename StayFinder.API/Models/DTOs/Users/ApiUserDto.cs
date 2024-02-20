using System.ComponentModel.DataAnnotations;

namespace StayFinder.API.Models.DTOs.Users
{
    public class ApiUserDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
