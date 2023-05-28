using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Controllers;

public class RaceController : Controller
{
    private readonly ApplicationDbContext _context;

    public RaceController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET
    public IActionResult Index()
    {
        List<Race> races = _context.Races.ToList();
        return View();
    }
}