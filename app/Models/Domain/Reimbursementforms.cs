#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reimbursementform.Models
{
    public class Reimbursementforms
    {
        [Key]
        public int FormId {get; set;}
        
        [StringLength(60, MinimumLength = 3)]
        [Required(ErrorMessage = "Please enter the Name")]
        [Display(Name = "Name")]
        public string Name {get; set;}

        [Required(ErrorMessage = "please enter Email")]
        [Display(Name = "Email")]
        public string EmailId {get; set;}

        [Required(ErrorMessage = "Please enter the Mobile Number")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number")]

        [Display(Name = "Mobile")]
        public long Mobilenumber {get; set;}

        [Required(ErrorMessage = "Please fill the box")]
        [Display(Name = "ModeofTransport")]        
        public string ModeofTransport { get; set; }

        [Required(ErrorMessage = "Please enter the Amount")]
        [Display(Name = "Amount")]
        public int Amount { get; set; }

        [Display(Name = "Purpose")]
        public string Purpose { get; set; }

        [Required(ErrorMessage = "Please insert the file")]
        [Display(Name = "Receipt")]
        public string Receipt { get; set; }

        [NotMapped]
        public IFormFile Image {get; set;}

        [Required]
        public DateTime Date { get; set; }
        
        public string Description { get; set; }
        public string Status {get; set; }

        
    }
}