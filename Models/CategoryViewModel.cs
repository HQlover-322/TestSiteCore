using System.ComponentModel.DataAnnotations;

namespace TEST.Models
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Decription")]
        public string Description { get; set; }
        public DateTime CreatedAt;
        public DateTime UpdatedAt;
    }
}
