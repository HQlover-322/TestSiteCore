using Microsoft.AspNetCore.Mvc;
using TEST.Models;

namespace TEST.Controllers
{
    public class AdminController:Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

    }
}
