using Microsoft.AspNetCore.Mvc;

namespace Lithium.Api.Gallery.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<GalleryController> logger;

    public AccountController(ILogger<GalleryController> logger)
    {
        this.logger = logger;
    }

    [HttpPost("/account/login")]
    public IActionResult Login(string username, string password)
    {
        return username == "rolandz" && password == "1qazxsw2"
            ? Ok() : Unauthorized();
    }

    [HttpPost("/account/logout")]
    public async Task Logout()
    {
    }
}
