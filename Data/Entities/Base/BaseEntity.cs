namespace TEST.Data.Entities.Base
{
    public abstract class BaseEntity : IBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
