using Microsoft.AspNetCore.Mvc;
using TEST.Models;
using TEST.Services;


namespace TEST.Controllers
{
    public class AdminController:Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly CategoryService  _categoryService;
        private readonly ArticleService _articleService;
        private readonly TagService _tagService;
        public AdminController(ILogger<AdminController> logger, CategoryService categoryService, ArticleService articleService, TagService tagService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _articleService = articleService;
            _tagService = tagService;
        }
        #region Article
        public async Task<IActionResult> MyArticles()
        {
            (List<ArticleViewModel>, List<CategoryViewModel>) Models = (await _articleService.GetArticles(), await _categoryService.GetCategores());
            var ViewModel = new ArticleWithCategoryViewModel()
            {
                Articles = await _articleService.GetArticles(),
                Category = await _categoryService.GetCategores()
            };
            return View(Models);
        }
        public  async Task<IActionResult> AddNewArticle()
        {
            return View(await _categoryService.GetCategores());
        }
        [HttpPost]
        public async Task<IActionResult> AddNewArticle(ArticleViewModel model)
        {
            await _articleService.AddNewArticle(model);
            return View();
        }
        public async Task<IActionResult> ArticleDetailsUpdate(Guid id)
        {
            var model = new ArticleUpdateModel() { Article=await _articleService.GetArticleById(id), Categores= await _categoryService.GetCategores()};
            return View(model);

        }
        public async Task<IActionResult> MyArticlesUpdate(ArticleViewModel model)
        {
            await _articleService.UpdateArticle(model);
            (List<ArticleViewModel>, List<CategoryViewModel>) Models = (await _articleService.GetArticles(), await _categoryService.GetCategores());
            var ViewModel = new ArticleWithCategoryViewModel()
            {
                Articles = await _articleService.GetArticles(),
                Category = await _categoryService.GetCategores()
            };
            return View("MyArticles", Models);
        }
        public async Task<IActionResult> MyArticlesSortByCategory(Guid id)
        {
            (List<ArticleViewModel>, List<CategoryViewModel>) Models = (await _articleService.GetNewArticleByPredicate(x => x.CategoryId == id), await _categoryService.GetCategores());
            var ViewModel = new ArticleWithCategoryViewModel()
            {
                Articles = await _articleService.GetNewArticleByPredicate(x => x.CategoryId == id),
                Category = await _categoryService.GetCategores()
            };
            return View("MyArticles", Models);
        }
        public async Task<IActionResult> MyArticlesSortByDate(DateTime DateStart, DateTime DateEnd)
        {
            var ViewModel = new ArticleWithCategoryViewModel();
            (List<ArticleViewModel>, List<CategoryViewModel>) Models = (default,await _categoryService.GetCategores());
            if (DateEnd.Equals(default(DateTime)))
            {
                Models.Item1= await _articleService.GetNewArticleByPredicate(x => x.CreatedAt.Date.Equals(DateStart));
                ViewModel.Articles = await _articleService.GetNewArticleByPredicate(x => x.CreatedAt.Date.Equals(DateStart));
                ViewModel.Category = await _categoryService.GetCategores();                
            }
            else
            {
                Models.Item1 = await _articleService.GetNewArticleByPredicate(x => x.CreatedAt.Date >= DateStart && x.CreatedAt.Date <= DateEnd); 
                ViewModel.Articles = await _articleService.GetNewArticleByPredicate(x =>x.CreatedAt.Date>=DateStart && x.CreatedAt.Date<=DateEnd);
                ViewModel.Category = await _categoryService.GetCategores();
            }
            return View("MyArticles", ViewModel);
        }

        #endregion

        #region Category
        public async Task<IActionResult> Category()
        {
            return View(await _categoryService.GetCategores());
        }
        public IActionResult AddNewCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNewCategory(CategoryViewModel model)
        {
            await _categoryService.AddNewCategory(model);
            return View();
        }
        public async Task<IActionResult> CategoryDetailsUpdate(Guid id)
        {
            var model = await _categoryService.GetCategoryById(id);
            return View(model);
        }
        public async Task<IActionResult> CategoryUpdate(CategoryViewModel model)
        {
            await _categoryService.UpdateCategory(model);
            return View("Category", await _categoryService.GetCategores());
        }
        #endregion
        #region Tags
        public IActionResult AddNewTag()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNewTag(TagViewModel model)
        {
            await _tagService.AddNewTag(model);
            return View();
        }
        #endregion
    }
}
