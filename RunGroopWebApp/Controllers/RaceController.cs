using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using RunGroopWebApp.Services;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class RaceController : Controller
{
    private readonly IRaceRepository _raceRepository;
    private readonly IPhotoService _photoService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RaceController(IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
    {
        _raceRepository = raceRepository;
        _photoService = photoService;
        _httpContextAccessor = httpContextAccessor;
    }
    // GET
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<Race> races = await _raceRepository.GetAll();
        return View(races);
    }

    [HttpGet]
    public async Task<IActionResult>  Detail(int id)
    {
        Race race = await _raceRepository.GetByIdAsync(id);
        return View(race);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
        var createRaceViewModel = new CreateRaceViewModel
        {
            AppUserId = currentUserId
        };
        return View(createRaceViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(raceVM.Image);

            var race = new Race
            {
                Title = raceVM.Title,
                Description = raceVM.Description,
                Image = result.Url.ToString(),
                AppUserId = raceVM.AppUserId,
                RaceCategory = raceVM.RaceCategory,
                Address = new Address
                {
                    Street = raceVM.Address.Street,
                    City = raceVM.Address.City,
                    State = raceVM.Address.State
                }
            };
            _raceRepository.Add(race);
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("", "Photo upload failed");
        }

        return View(raceVM);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var race = await _raceRepository.GetByIdAsync(id);
        if (race == null) return View("Error");
        var raceVM = new EditRaceViewModel
        {
            Title = race.Title,
            Description = race.Description,
            AddressId = race.AddressId,
            Address = race.Address,
            URL = race.Image,
            RaceCategory = race.RaceCategory
        };
        return View(raceVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit club");
            return View(raceVM);
        }

        var userRace = await _raceRepository.GetByIdAsyncNoTracking(id);

        if (userRace == null)
        {
            return View("Error");
        }

        var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);

        if (photoResult.Error != null)
        {
            ModelState.AddModelError("Image", "Photo upload failed");
            return View(raceVM);
        }

        if (!string.IsNullOrEmpty(userRace.Image))
        {
            _ = _photoService.DeletePhotoAsync(userRace.Image);
        }

        var race = new Race
        {
            Id = id,
            Title = raceVM.Title,
            Description = raceVM.Description,
            Image = photoResult.Url.ToString(),
            AddressId = raceVM.AddressId,
            Address = raceVM.Address,
        };

        _raceRepository.Update(race);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var raceDetail = await _raceRepository.GetByIdAsync(id);
        if (raceDetail == null) return View("Error");
        return View(raceDetail);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteRace(int id)
    {
        var raceDetail = await _raceRepository.GetByIdAsync(id);
        if (raceDetail == null) return View("Error");
        _raceRepository.Delete(raceDetail);
        return RedirectToAction("Index");
    }
}