using Microsoft.AspNetCore.Mvc;

namespace Lithium.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<OldGalleryController> logger;

    public AccountController(ILogger<OldGalleryController> logger)
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
