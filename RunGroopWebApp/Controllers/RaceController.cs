using Microsoft.AspNetCore.Mvc;

namespace RunGroopWebApp.Controllers;

public class RaceController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}