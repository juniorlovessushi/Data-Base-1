namespace RaceDay.API.Services;

public interface IBlobService
{
    Task<string> UploadFileAsync(IFormFile file, string containerName);
}