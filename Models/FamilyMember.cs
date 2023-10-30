using System.ComponentModel.DataAnnotations;

namespace DadsInventory.Models
{
    public class FamilyMember
    {
        [Required(ErrorMessage = "Field Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field Password is required.")]
        public string Password { get; set; }
        public string[] Roles { get; set; }
    }
}