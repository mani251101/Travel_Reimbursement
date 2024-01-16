using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
#nullable disable

namespace application
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public DateTime DateofBirth {get; set;}

        [NotMapped]
        public IFormFile ProfilePictureFile { get; set; }
        public string ProfilePicture {get; set;}
    }
}