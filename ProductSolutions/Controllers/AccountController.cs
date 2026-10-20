using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductSolutions.Models;

namespace ProductSolutions.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
