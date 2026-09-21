using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;
using RaceDay.Web.ViewModels;

namespace RaceDay.Web.Controllers;

public class ParticipantController : Controller
{
    private readonly ApiService _apiService;

    public ParticipantController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public async Task<IActionResult> RegisterRace(int eventId)
    {
        var categories = await _apiService.GetCategoriesByEventAsync(eventId);
        ViewBag.Categories = categories;
        return View(new CreateEnrolmentViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> RegisterRace(CreateEnrolmentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var success = await _apiService.EnrollParticipantAsync(model);
        if (success)
        {
            return RedirectToAction("MyRaces");
        }

        ModelState.AddModelError(string.Empty, "Failed to submit enrolment.");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> MyRaces()
    {
        var races = await _apiService.GetMyRacesAsync();
        return View(races);
    }
}