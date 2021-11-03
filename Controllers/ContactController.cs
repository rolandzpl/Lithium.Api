using Microsoft.AspNetCore.Mvc;

namespace Lithium.Api.Gallery.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    [HttpPost]
    public void SendMessage() { }
}
