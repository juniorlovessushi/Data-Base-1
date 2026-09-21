using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;
using RaceDay.Web.ViewModels;

namespace RaceDay.Web.Controllers;

public class OrganiserController : Controller
{
    private readonly ApiService _apiService;

    public OrganiserController(ApiService apiService)
    {
        _apiService = apiService;
    }

    // =========================
    // ORGANISER DASHBOARD
    // =========================
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var events = await _apiService.GetEventsAsync();

        return View(events);
    }


    // =========================
    // CREATE EVENT - GET
    // =========================
    [HttpGet]
    public IActionResult CreateEvent()
    {
        return View(new CreateEventViewModel());
    }


    // =========================
    // CREATE EVENT - POST
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEvent(
        CreateEventViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var eventId =
            await _apiService.CreateEventAsync(model);

        if (eventId.HasValue)
        {
            return RedirectToAction(nameof(Dashboard));
        }

        ModelState.AddModelError(
            "",
            "Failed to create the event. Please try again."
        );

        return View(model);
    }
}