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
        private readonly HeroImage  _heroImage;

        public ArticleService(EfDBContex dbContex, CategoryService categoryService)
        {
            this.dbContex = dbContex;
            this.categoryService = categoryService;
        }
        public async Task<Article> AddNewArticle(ArticleViewModel data)
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
            return item;

        }
        public async Task<List<ArticleViewModel>> GetArticles()
        {
            var articles = await dbContex.Articles.Include(x=>x.HeroImage).ToListAsync();
            return articles.Select(x => new ArticleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                CategoryId = x.CategoryId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                HeroImagePath=x.HeroImage?.Path
            }).ToList();
        }
        public async Task<ArticleViewModel> GetArticleById(Guid id)
        {
            var article = await dbContex.Articles
                .Include(x => x.Tags)
                .Include(x=>x.HeroImage)
                .FirstOrDefaultAsync(x => x.Id == id);
            return new ArticleViewModel()
            {
                Id = article.Id,
                Name = article.Name,
                ShortDescription = article.ShortDescription,
                Description = article.Description,
                CategoryId = article.CategoryId,
                HeroImagePath = article.HeroImage?.Path,
                CreatedAt = article.CreatedAt,
                Tags = article.Tags.Select(x => new TagViewModel() { Id = x.Id, Name = x.Name }).ToList(),
                UpdatedAt = article.UpdatedAt
            };
        }
        public async Task<Article> UpdateArticle(ArticleViewModel data, Guid[] Tags)
        {
            var item = await dbContex.Articles
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == data.Id);
            if (item is not null)
            {
                var tags = dbContex.Tags.Where(x => Tags.Contains(x.Id));
                item.Name = data.Name;
                item.ShortDescription = data.ShortDescription;
                item.Description = data.Description;
                item.CategoryId = data.CategoryId;
                item.UpdatedAt = DateTime.Now;

                foreach (var existTag in item.Tags)
                    if (!Tags.Contains(existTag.Id))
                        item.Tags.Remove(existTag);

                foreach (var tag in tags)
                    item.Tags.Add(tag);
            }

            dbContex.Update(item);
            dbContex.SaveChanges();
            return item;
        }
        public async Task<List<ArticleViewModel>> GetNewArticleByPredicate(Expression<Func<Article, bool>> expression, PageConfig config)
        {
            var tasks = await dbContex.Articles
                .Include(x=>x.HeroImage)
                .Where(expression)
                .Skip((config.CurrentPage - 1) * config.PageSize)
                .Take(config.PageSize)
                .ToListAsync();
            return tasks.Select(x => new ArticleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                CategoryId = x.CategoryId,
                CreatedAt = x.CreatedAt,
                HeroImagePath = x.HeroImage?.Path,
                UpdatedAt = x.UpdatedAt
            }).ToList();
        }
        public async Task ArticleRemove(Guid id)
        {
            using(dbContex)
            {
                var item = await dbContex.Articles.FindAsync(id);
                dbContex.Remove(item);
                dbContex.SaveChanges();
            }
        }
        public async Task<int> GetCount()
        {
                return await dbContex.Articles.CountAsync();
        }
        public async  Task<List<ArticleViewModel>> GetArticles(PageConfig config)
        {
            var articles = await dbContex.Articles.Skip((config.CurrentPage - 1) * config.PageSize).Take(config.PageSize).Include(x => x.HeroImage).ToListAsync();
            return articles.Select(x => new ArticleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                CategoryId = x.CategoryId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                HeroImagePath = x.HeroImage?.Path
            }).ToList();
        }
    }
}
