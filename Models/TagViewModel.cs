using System.ComponentModel.DataAnnotations;

namespace TEST.Models
{
    public class TagViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
