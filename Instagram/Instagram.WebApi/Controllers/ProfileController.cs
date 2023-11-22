using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.WebApi.Controllers;

[Authorize]
[Route("profile")]
public class ProfileController : ApiController
{
    [HttpGet]
    public IActionResult Profile()
    {
        return Ok(Array.Empty<string>());
    }
}