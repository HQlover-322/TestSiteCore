using TEST.Data.Entities;

namespace TEST.Models
{
    public class ArticleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IFormFile HeroImage { get; set; }
        public string HeroImagePath { get; set; }
        public Guid CategoryId { get; set; }
        public List<TagViewModel> Tags { get; set; }
    }
}
