using TEST.Data.Entities.Base;

namespace TEST.Data.Entities
{
    public class Article: BaseEntity
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public HeroImage HeroImage { get; set; }
        public Guid? HeroImageId { get; set; }
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public User Author { get; set; }
        public string AuthorId { get; set; }
    }
}
