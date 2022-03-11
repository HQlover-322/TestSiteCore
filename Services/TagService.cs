using Microsoft.EntityFrameworkCore;
using TEST.Data.Entities;
using TEST.Models;

namespace TEST.Services
{
    public class TagService
    {
        private readonly EfDBContex dbContex;

        public TagService(EfDBContex dbContex)
        {
            this.dbContex = dbContex;
        }
        public async Task AddNewTag(TagViewModel data)
        {
            using (dbContex)
            {
                var item = new Data.Entities.Tag()
                {
                    Name = data.Name,
                };
                dbContex.Tags.Add(item);
                await dbContex.SaveChangesAsync();
            }
        }
        public async Task<List<TagViewModel>> GetTags()
        {
            var articles = await dbContex.Tags.AsNoTracking().ToListAsync();
            return articles.Select(x => new TagViewModel()
            {
                Id= x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();
        }
        public async Task TagRemove(Guid id)
        {
            using (dbContex)
            {
                var item = await dbContex.Tags.FindAsync(id);
                dbContex.Remove(item);
                dbContex.SaveChanges();
            }
        }
    }
}
