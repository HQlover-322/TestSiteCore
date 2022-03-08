using Microsoft.EntityFrameworkCore;
using TEST.Data.Entities;
using TEST.Models;

namespace TEST.Services
{
    public class CategoryService
    {
        private readonly EfDBContex dbContex;

        public CategoryService(EfDBContex dbContex)
        {
            this.dbContex = dbContex;
        }
        public async Task<List<CategoryViewModel>> GetCategores()
        {
            var categories = await dbContex.Categories.AsNoTracking().ToListAsync();
            return categories.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();
        }
        public async Task AddNewCategory(CategoryViewModel data)
        {
            using(dbContex)
            {
                var item = new Data.Entities.Category()
                {
                    Name = data.Name,
                    Description = data.Description
                };
                dbContex.Categories.Add(item);
                await dbContex.SaveChangesAsync(); 
            }
        }
        public async Task UpdateCategory(CategoryViewModel data)
        {
            var item = await dbContex.Categories.FindAsync(data.Id);
            if (item is not null)
            {
                item.Name = data.Name;
                item.Description = data.Description;
                item.UpdatedAt = DateTime.Now;
            }
            dbContex.Update(item);
            dbContex.SaveChanges();
        }
        public async Task<CategoryViewModel> GetCategoryById(Guid id)
        {
            var categories = await dbContex.Categories.AsNoTracking().ToListAsync();
            return categories.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).FirstOrDefault(x => x.Id == id);
        }
    }
}
