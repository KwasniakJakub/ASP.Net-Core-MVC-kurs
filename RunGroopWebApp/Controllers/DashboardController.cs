using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
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

    private void MapUserEdit(AppUser user,
        EditUserDashboardViewModel editVM,
        ImageUploadResult photoResult)
    {
        user.Id = editVM.Id;
        user.Pace = editVM.Pace;
        user.Mileage = editVM.Mileage;
        user.ProfileImageUrl = photoResult.Url.ToString();
        user.City = editVM.City;
        user.State = editVM.State;
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

        AppUser user = await _dashboardRepository.GetByIdNoTracking(editVM.Id);

        if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
        {
            var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
            //Optimistic Concurrency = "Tracking error"
            //Use No Tracking
            MapUserEdit(user, editVM, photoResult);

            _dashboardRepository.Update(user);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            try
            {
                await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","Could not delete photo");
                return View(editVM);
            }
            var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
            MapUserEdit(user, editVM, photoResult);
            _dashboardRepository.Update(user);
            return RedirectToAction("Index", "Home");
        }
    }
}