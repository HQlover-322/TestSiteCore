using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEST.Models;
using TEST.Services;


namespace TEST.Controllers
{
    [Authorize]
    public class AdminController:Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly CategoryService  _categoryService;
        private readonly ArticleService _articleService;
        private readonly TagService _tagService;
        private readonly HeroImageService _heroImageService;
        public AdminController(ILogger<AdminController> logger, CategoryService categoryService, ArticleService articleService, TagService tagService, HeroImageService heroImageService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _articleService = articleService;
            _tagService = tagService;
            _heroImageService = heroImageService;
        }
        #region Article
        public async Task<IActionResult> MyArticles()
        {
            (List<ArticleViewModel>, List<CategoryViewModel>) Models = (await _articleService.GetArticles(), await _categoryService.GetCategores());
            return View(Models);
        }
        public  async Task<IActionResult> AddNewArticle()
        {
            return View(await _categoryService.GetCategores());
        }
        [HttpPost]
        public async Task<IActionResult> AddNewArticle(ArticleViewModel model)
        {
            var article= await _articleService.AddNewArticle(model);
            if (model.HeroImage is not null)
            await _heroImageService.AddNewImage(model.HeroImage, article);
            return RedirectToAction(nameof(AddNewArticle));
        }
        public async Task<IActionResult> ArticleDetailsUpdate(Guid id)
        {
            var model = await _articleService.GetArticleById(id);
            ViewBag.Categores = await _categoryService.GetCategores();
            ViewBag.Tags = await _tagService.GetTags();
            return View(model);
        }
        public async Task<IActionResult> MyArticlesUpdate(ArticleViewModel model, Guid[] Tags)
        {
            await _articleService.UpdateArticle(model,Tags);
            return RedirectToAction(nameof(MyArticles));
        }
        public async Task<IActionResult> MyArticlesSortByCategory(Guid id)
        {
            (List<ArticleViewModel>, List<CategoryViewModel>) Models = (await _articleService.GetNewArticleByPredicate(x => x.CategoryId == id), await _categoryService.GetCategores());
            return View("MyArticles", Models);
        }
        public async Task<IActionResult> MyArticlesSortByDate(DateTime DateStart, DateTime DateEnd)
        {
            (List<ArticleViewModel>, List<CategoryViewModel>) Models = (default,await _categoryService.GetCategores());
            if (DateEnd.Equals(default(DateTime)))
            {
                Models.Item1= await _articleService.GetNewArticleByPredicate(x => x.CreatedAt.Date.Equals(DateStart));
            }
            else
            {
                Models.Item1 = await _articleService.GetNewArticleByPredicate(x => x.CreatedAt.Date >= DateStart && x.CreatedAt.Date <= DateEnd); 
            }
            return View("MyArticles", Models);
        }
        public async Task<IActionResult> MyArticleRemove(Guid id)
        {
            await _articleService.ArticleRemove(id);
            return RedirectToAction(nameof(MyArticles));
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
            return RedirectToAction(nameof(Category));
        }
        public async Task<IActionResult> MyCategoryRemove(Guid id)
        {
            await _categoryService.CategoryRemove(id);
            return RedirectToAction(nameof(Category));
        }
        //Удаление каскадное, пизда
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
