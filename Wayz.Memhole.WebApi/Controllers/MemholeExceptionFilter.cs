using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Wayz.Memhole.Kernel;

namespace Wayz.Memhole.WebApi;
public class MemholeExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MemholeInvalidDeviceException)
        {
            context.Result = new JsonResult(new { error = "Invalid device." });
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        else if (context.Exception is MemholeDeviceNotFoundException)
        {
            context.Result = new JsonResult(new { error = "Device not found." });
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        else if (context.Exception is MemholeDeviceBusyException)
        {
            context.Result = new JsonResult(new { error = "Device is busy." });
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
        }
        else if (context.Exception is MemholeInvalidPidException)
        {
            context.Result = new JsonResult(new { error = "Invalid process ID." });
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        else if (context.Exception is MemholeKernelMallocFailureException)
        {
            context.Result = new JsonResult(new { error = "Kernel memory allocation failure." });
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
        else if (context.Exception is MemholeUnsupportedOperationException)
        {
            context.Result = new JsonResult(new { error = "Unsupported operation." });
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        else
        {
            return;
        }

        context.ExceptionHandled = true;
    }
}
