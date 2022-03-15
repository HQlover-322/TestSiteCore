using System.ComponentModel.DataAnnotations;
using TEST.Data.Entities;

namespace TEST.Models
{
    public class ArticleViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "ShortDescription")]
        public string ShortDescription { get; set; }
        [Display(Name = "Description")]
        [Required]
        [MaxLength(1024)]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IFormFile HeroImage { get; set; }
        public string HeroImagePath { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        public List<TagViewModel> Tags { get; set; }
    }
}
