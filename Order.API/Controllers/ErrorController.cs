using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers
{
    // This Controller is used to handle the exceptions that occurs in the production environment
    [ApiController]
    public class ErrorController : ControllerBase
    {
        // Exception handler for prod environment.
        [Route("/error")]
        // This will omit this api from swagger documentation
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ErrorProdEnv() => Problem();
       
    }
}
