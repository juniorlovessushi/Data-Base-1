using System.Net.Http.Headers;
using System.Net.Http.Json;
using RaceDay.Web.ViewModels;

namespace RaceDay.Web.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // =========================
    // GET ALL EVENTS
    // =========================
    public async Task<List<EventViewModel>> GetEventsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/events");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"GetEvents Error: {response.StatusCode}"
                );

                return new List<EventViewModel>();
            }

            var events =
                await response.Content.ReadFromJsonAsync<List<EventViewModel>>();

            return events ?? new List<EventViewModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"GetEvents Exception: {ex.Message}"
            );

            return new List<EventViewModel>();
        }
    }


    // =========================
    // CREATE EVENT
    // =========================
    public async Task<int?> CreateEventAsync(CreateEventViewModel model)
    {
        try
        {
            // The API expects JSON, not multipart form data.
            var createEventDto = new
            {
                EventName = model.EventName,
                EventDate = model.EventDate,
                Location = model.Location
            };

            var response =
                await _httpClient.PostAsJsonAsync(
                    "api/events",
                    createEventDto
                );

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                Console.WriteLine(
                    $"CreateEvent Error: {response.StatusCode} - {error}"
                );

                return null;
            }

            var createdEvent =
                await response.Content
                    .ReadFromJsonAsync<EventViewModel>();

            if (createdEvent == null)
            {
                return null;
            }

            // Upload the banner separately after the event exists.
            if (model.BannerImage != null &&
                model.BannerImage.Length > 0)
            {
                await UploadEventBannerAsync(
                    createdEvent.EventId,
                    model.BannerImage
                );
            }

            return createdEvent.EventId;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"CreateEvent Exception: {ex.Message}"
            );

            return null;
        }
    }


    // =========================
    // UPLOAD EVENT BANNER
    // =========================
    private async Task<bool> UploadEventBannerAsync(
        int eventId,
        IFormFile file)
    {
        try
        {
            using var content =
                new MultipartFormDataContent();

            using var stream =
                file.OpenReadStream();

            using var streamContent =
                new StreamContent(stream);

            streamContent.Headers.ContentType =
                new MediaTypeHeaderValue(
                    file.ContentType
                );

            content.Add(
                streamContent,
                "file",
                file.FileName
            );

            var response =
                await _httpClient.PostAsync(
                    $"api/events/{eventId}/upload-banner",
                    content
                );

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                Console.WriteLine(
                    $"UploadBanner Error: {response.StatusCode} - {error}"
                );

                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"UploadBanner Exception: {ex.Message}"
            );

            return false;
        }
    }


    // =========================
    // GET CATEGORIES FOR EVENT
    // =========================
    public async Task<List<CategoryViewModel>> GetCategoriesByEventAsync(
        int eventId)
    {
        try
        {
            var response =
                await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>(
                    $"api/categories/event/{eventId}"
                );

            return response ?? new List<CategoryViewModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"GetCategories Error: {ex.Message}"
            );

            return new List<CategoryViewModel>();
        }
    }


    // =========================
    // ENROL PARTICIPANT
    // =========================
    public async Task<bool> EnrollParticipantAsync(
        CreateEnrolmentViewModel model)
    {
        try
        {
            var response =
                await _httpClient.PostAsJsonAsync(
                    "api/enrolments",
                    model
                );

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"EnrollParticipant Error: {ex.Message}"
            );

            return false;
        }
    }


    // =========================
    // GET MY RACES
    // =========================
    public async Task<List<EnrolmentViewModel>> GetMyRacesAsync()
    {
        try
        {
            var response =
                await _httpClient.GetFromJsonAsync<List<EnrolmentViewModel>>(
                    "api/enrolments/my-races"
                );

            return response ?? new List<EnrolmentViewModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"GetMyRaces Error: {ex.Message}"
            );

            return new List<EnrolmentViewModel>();
        }
    }
}