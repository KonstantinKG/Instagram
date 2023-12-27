using System.Security.Claims;

using ErrorOr;
using Instagram.WebApi.Common.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.WebApi.Controllers;

[ApiController]
[Authorize]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        var firstError = errors[0];
        
        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        HttpContext.Items[HttpContextItemKeys.Errors] = errors;
        
        return Problem(statusCode: statusCode);
    }

    protected T GetUserClaim<T>(Func<Claim,bool> predicate)
    {
        var claim = HttpContext.User.Claims.First(predicate).Value;
        if (claim is T tClaim)
            return tClaim;
        return (T)Convert.ChangeType(claim, typeof(T));
    }
}