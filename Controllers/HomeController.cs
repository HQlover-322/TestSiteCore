using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TEST.Models;
using TEST.Services;

namespace TEST.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ArticleService _articleService;

    public HomeController(ILogger<HomeController> logger, ArticleService articleService)
    {
        _logger = logger;
        _articleService = articleService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public async Task<IActionResult> ArticleList()
    {
        return View(await _articleService.GetArticles());
    }
    public async Task<IActionResult> ArticleDetails(Guid id)
    {
        return View(await _articleService.GetArticleById(id));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
