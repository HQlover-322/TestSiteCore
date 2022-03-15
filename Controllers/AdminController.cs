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
        public async Task<IActionResult> MyArticles(PageConfig config)
        {
            PageViewModel<ArticleViewModel> pageViewModel = new PageViewModel<ArticleViewModel>()
            {
                CurrentPage = config.CurrentPage,
                Items = await _articleService.GetArticles(config),
                PageSize = config.PageSize,
                TotalPages = await _articleService.GetCount() / config.PageSize
            };
            (PageViewModel<ArticleViewModel>, List<CategoryViewModel>) Models = (pageViewModel, await _categoryService.GetCategores());
            ViewBag.Action = nameof(MyArticles);
            ViewBag.Tags = await _tagService.GetTags();
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
            var article = await _articleService.UpdateArticle(model,Tags);
            if (model.HeroImage is not null)
                await _heroImageService.AddNewImage(model.HeroImage, article);
            return RedirectToAction(nameof(MyArticles));
        }
        public async Task<IActionResult> MyArticlesSortByCategory(Guid id,PageConfig config)
        {
            PageViewModel<ArticleViewModel> pageViewModel = new PageViewModel<ArticleViewModel>()
            {
                CurrentPage = config.CurrentPage,
                Items = await _articleService.GetNewArticleByPredicate(x => x.CategoryId == id,config),
                PageSize = config.PageSize,
                TotalPages = await _articleService.GetCount() / config.PageSize
            };
            (PageViewModel<ArticleViewModel>, List<CategoryViewModel>) Models = (pageViewModel, await _categoryService.GetCategores());
            ViewBag.Action = nameof(MyArticlesSortByCategory);
            ViewBag.Tags = await _tagService.GetTags();
            return View(Models);
        }
        public async Task<IActionResult> MyArticlesSortByTags(Guid[] tags, PageConfig config)
        {
            PageViewModel<ArticleViewModel> pageViewModel = new PageViewModel<ArticleViewModel>()
            {
                CurrentPage = config.CurrentPage,
                Items = await _articleService.GetNewArticleByPredicate(x=>x.Tags.Any(y=>tags.Contains(y.Id)), config),
                PageSize = config.PageSize,
                TotalPages = await _articleService.GetCount() / config.PageSize
            };
            (PageViewModel<ArticleViewModel>, List<CategoryViewModel>) Models = (pageViewModel, await _categoryService.GetCategores());
            ViewBag.Action = nameof(MyArticlesSortByTags);
            ViewBag.Tags = await _tagService.GetTags();
            ViewBag.SelectedTags = tags;
            return View(Models);
        }
        public async Task<IActionResult> MyArticlesSortByDate(DateTime DateStart, DateTime DateEnd, PageConfig config)
        {
            PageViewModel<ArticleViewModel> pageViewModel = new PageViewModel<ArticleViewModel>()
            {
                CurrentPage = config.CurrentPage,
                PageSize = config.PageSize,
                TotalPages = await _articleService.GetCount() / config.PageSize
            };
            (PageViewModel<ArticleViewModel>, List<CategoryViewModel>) Models = (pageViewModel, await _categoryService.GetCategores());
            if (DateEnd.Equals(default(DateTime)))
            {
                Models.Item1.Items= await _articleService.GetNewArticleByPredicate(x => x.CreatedAt.Date.Equals(DateStart),config);
            }
            else
            {
                Models.Item1.Items = await _articleService.GetNewArticleByPredicate(x => x.CreatedAt.Date >= DateStart && x.CreatedAt.Date <= DateEnd,config); 
            }
            if(DateStart!=default)
            ViewBag.DateStart = DateStart.ToString("yyyy-MM-dd") ;
            if (DateEnd != default)
                ViewBag.DateEnd = DateEnd.ToString("yyyy-MM-dd");
            ViewBag.Tags = await _tagService.GetTags();
            ViewBag.Action = nameof(MyArticlesSortByDate);
            return View(Models);
        }
        public async Task<IActionResult> MyArticleRemove(Guid id)
        {
            await _articleService.ArticleRemove(id);
            return RedirectToAction(nameof(MyArticles));
        }

        #endregion

        #region Category
        public async Task<IActionResult> Category(PageConfig config)
        {
            PageViewModel<CategoryViewModel> pageViewModel = new PageViewModel<CategoryViewModel>()
            {
                CurrentPage = config.CurrentPage,
                Items = await _categoryService.GetCategores(config),
                PageSize = config.PageSize,
                TotalPages = await _categoryService.GetCount() / config.PageSize
            };
            return View(pageViewModel);
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
        #endregion

        #region Tags
        public async Task<IActionResult> Tags(PageConfig config)
        {
            PageViewModel<TagViewModel> pageViewModel = new PageViewModel<TagViewModel>()
            {
                CurrentPage = config.CurrentPage,
                Items = await _tagService.GetTags(config),
                PageSize = config.PageSize,
                TotalPages = await _tagService.GetCount() / config.PageSize
            };
            return View(pageViewModel);
        }
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
        public async Task<IActionResult> TagRemove(Guid id)
        {
            await _tagService.TagRemove(id);
            return RedirectToAction(nameof(Tags));
        }

        #endregion
    }
}
