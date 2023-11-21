using Microsoft.AspNetCore.Mvc;

namespace Instagram.WebApi.Controllers;

public class ErrorController : ApiController
{
    [Route("/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error()
    {
        return Problem();
    }
}