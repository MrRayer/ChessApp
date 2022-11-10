using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChessApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
