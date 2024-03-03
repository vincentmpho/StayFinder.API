using Microsoft.AspNetCore.Identity;

namespace StayFinder.API.Models
{
    public class ApiUser :IdentityUser
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
           


    }
}
