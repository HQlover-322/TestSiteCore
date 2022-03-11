using TEST.Data.Entities.Base;

namespace TEST.Data.Entities
{
    public class HeroImage : BaseEntity
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Article Article { get; set; }
        public Guid ArticleId { get; set; }
    }
}
