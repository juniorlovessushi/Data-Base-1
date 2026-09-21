using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;
using RaceDay.Web.ViewModels;
using System.Net.Http.Json;

namespace RaceDay.Web.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _client;

    public AccountController(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("RaceDayAPI");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // Send login credentials to backend API
        var response = await _client.PostAsJsonAsync("api/Auth/login", model);

        if (response.IsSuccessStatusCode)
        {
            // Save authentication state in Session
            HttpContext.Session.SetString("IsLoggedIn", "true");
            return RedirectToAction("DashBoard", "Organiser");
        }

        ModelState.AddModelError(string.Empty, "Invalid login credentials.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}