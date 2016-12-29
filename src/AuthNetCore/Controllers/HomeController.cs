﻿using Microsoft.AspNetCore.Mvc;

namespace AuthNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
    }
}
