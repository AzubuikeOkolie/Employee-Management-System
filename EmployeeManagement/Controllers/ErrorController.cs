using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagement.Controllers
{
   
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        [Route("Error/{StatusCode}")]
        public IActionResult Index(int StatusCode)
        {
            var StatusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (StatusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource cannot be found";
                    ViewBag.Path = StatusCodeResult.OriginalPath;
                    ViewBag.QS = StatusCodeResult.OriginalQueryString; 
                    break;
            }
            return View("NotFound");
        }
        [Route("Error")]
        [AllowAnonymous]
       public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            logger.LogError($"The path {exceptionDetails.Path} threw an exception " +
                $"{exceptionDetails.Error}");

            return View("Error");

        } 
    }
          
}
