using TEST.Data.Entities.Base;

namespace TEST.Data.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
