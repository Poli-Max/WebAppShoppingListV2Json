using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web_App_Shop_V2.Models;

namespace Web_App_Shop_V2.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}

