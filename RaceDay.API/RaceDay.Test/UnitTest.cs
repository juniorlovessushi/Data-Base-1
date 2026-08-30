 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RaceDay.API.Controllers;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;
using Xunit;

namespace RaceDay.Tests;

public class UnitTests
{
    private RaceDayDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<RaceDayDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new RaceDayDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private IConfiguration GetMockConfig()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"Jwt:Key", "SuperSecretKeyThatIsAtLeast32BytesLong!"},
            {"Jwt:Issuer", "RaceDayAPI"},
            {"Jwt:Audience", "RaceDayUsers"}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task Register_ValidUser_Returns201Created()
    {
        var context = GetInMemoryDbContext();
        var controller = new AuthController(context, GetMockConfig());
        var registerDto = new RegisterDto("Test Organiser", "organiser@test.com", "Password123!", 1);

        var result = await controller.Register(registerDto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        var context = GetInMemoryDbContext();
        var authController = new AuthController(context, GetMockConfig());
        await authController.Register(new RegisterDto("John Doe", "john@test.com", "Password123!", 2));

        var loginDto = new LoginDto("john@test.com", "Password123!");
        var result = await authController.Login(loginDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthResponseDto>(okResult.Value);
        Assert.NotNull(response.Token);
    }

    [Fact]
    public async Task GetAllEvents_ReturnsAllEvents()
    {
        var context = GetInMemoryDbContext();
        context.Events.Add(new Event { EventId = 1, EventName = "City Marathon", EventDate = DateTime.Now, Location = "Downtown", OrganiserId = 1 });
        await context.SaveChangesAsync();

        var controller = new EventsController(context);
        var result = await controller.GetAllEvents();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var events = Assert.IsAssignableFrom<IEnumerable<EventResponseDto>>(okResult.Value);
        Assert.Single(events);
    }
}