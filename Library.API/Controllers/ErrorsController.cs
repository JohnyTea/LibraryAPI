using Library.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Library.API.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    [Route("/errors")]
    public MyErrorResponse Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context.Error;
        var code = 500;

        if (exception is ElementNotFoundException) code = 404;

        Response.StatusCode = code;

        return new MyErrorResponse(exception);
    }
}

