﻿using Microsoft.AspNetCore.Mvc;

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
