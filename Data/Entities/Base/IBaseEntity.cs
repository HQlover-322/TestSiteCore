namespace TEST.Data.Entities.Base
{
    public interface IBaseEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
