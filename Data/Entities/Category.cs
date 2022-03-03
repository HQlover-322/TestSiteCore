using TEST.Data.Entities.Base;

namespace TEST.Data.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
