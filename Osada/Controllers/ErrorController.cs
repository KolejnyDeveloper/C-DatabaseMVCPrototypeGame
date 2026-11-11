
using Microsoft.AspNetCore.Mvc;

namespace Osada.Controllers;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        Response.StatusCode = statusCode;

        return statusCode switch
        {
            400 => View("400"),
            401 => View("401"),
            403 => View("403"),
            404 => View("404"),
            500 => View("500"),
            _ => View("General")
        };
    }

    [Route("Error")]
    public IActionResult General()
    {
        Response.StatusCode = 500;
        return View("500");
    }
}
