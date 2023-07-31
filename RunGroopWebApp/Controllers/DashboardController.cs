using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardRepository _dashboardRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPhotoService _photoService;

    public DashboardController(IDashboardRepository dashboardRepository,
        IHttpContextAccessor httpContextAccessor,
        IPhotoService photoService)
    {
        _dashboardRepository = dashboardRepository;
        _httpContextAccessor = httpContextAccessor;
        _photoService = photoService;
    }

    public async Task<IActionResult> Index()
    {
        var userRaces = await _dashboardRepository.GetAllUserRaces();
        var userClubs = await _dashboardRepository.GetAllUserClubs();
        var dashboardViewModel = new DashboardViewModel()
        {
            Races = userRaces,
            Clubs = userClubs
        };
        return View(dashboardViewModel);
    }

    public async Task<IActionResult> EditUserProfile()
    {
        var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _dashboardRepository.GetUserById(currentUserId);
        if (user == null) return View("Error");
        var editUserViewmodel = new EditUserDashboardViewModel()
        {
            Id = currentUserId,
            Pace = user.Pace,
            Mileage = user.Mileage,
            ProfileImageUrl = user.ProfileImageUrl,
            City = user.City,
            State = user.City
        };
        return View(editUserViewmodel);
    }

    [HttpPost]
    public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("","Failed to edit profile");
            return View("EditUserProfile", editVM);
        }

        var user = await _dashboardRepository.GetByIdNoTracking(editVM.Id);
    }
}