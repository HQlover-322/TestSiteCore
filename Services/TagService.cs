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
    }
}
