using Microsoft.AspNetCore.Mvc;

namespace Lithium.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    [HttpPost]
    public void SendMessage() { }
}
