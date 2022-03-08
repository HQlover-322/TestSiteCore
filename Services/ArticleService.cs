using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TEST.Data.Entities;
using TEST.Models;

namespace TEST.Services
{
    public class ArticleService
    {
        private readonly EfDBContex dbContex;
        private readonly CategoryService categoryService;

        public ArticleService(EfDBContex dbContex, CategoryService categoryService)
        {
            this.dbContex = dbContex;
            this.categoryService = categoryService;
        }
        public async Task AddNewArticle(ArticleViewModel data)
        {
            using (dbContex)
            {
                var item = new Data.Entities.Article()
                {
                    Name = data.Name,
                    ShortDescription = data.ShortDescription,
                    Description = data.Description,
                    CategoryId = data.CategoryId
                };
                dbContex.Articles.Add(item);
                await dbContex.SaveChangesAsync();
            }
        }
        public async Task<List<ArticleViewModel>> GetArticles()
        {
               var articles = await dbContex.Articles.AsNoTracking().ToListAsync();
            return articles.Select(x => new ArticleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                CategoryId = x.CategoryId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();     
        }
        public async Task<ArticleViewModel> GetArticleById(Guid id)
        {
            var articles = await dbContex.Articles.AsNoTracking().ToListAsync();
            return articles.Select(x => new ArticleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                CategoryId = x.CategoryId,
                CreatedAt= x.CreatedAt,
                UpdatedAt= x.UpdatedAt
            }).FirstOrDefault(x=>x.Id==id);
            }
        public async Task UpdateArticle (ArticleViewModel data)
        {
            var item = await dbContex.Articles.FindAsync(data.Id);
            if(item is not null)
            {
                item.Name=data.Name;
                item.ShortDescription=data.ShortDescription;
                item.Description=data.Description;
                item.CategoryId=data.CategoryId;
                item.UpdatedAt=DateTime.Now;
            }
            dbContex.Update(item);
            dbContex.SaveChanges();
        }
        public async Task<List<ArticleViewModel>> GetNewArticleByPredicate(Expression<Func<Article, bool>> expression)
        {
            var tasks = await dbContex.Articles.AsNoTracking().Where(expression).ToListAsync();
            return tasks.Select(x => new ArticleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                CategoryId = x.CategoryId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();
        }
    }
}
