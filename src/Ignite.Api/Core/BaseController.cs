namespace Ignite.Api
{
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;    
    using System.Net;

    //[TypeFilter(typeof(AuthoriseResourceProvider), Order = -1)]
    public class BaseController : Controller
    {
        private readonly ILogger localLogger;

        public BaseController(ILoggerFactory loggerFactory)
        {
            localLogger = loggerFactory.CreateLogger<BaseController>();
        }

        protected JsonResult GetErrorJson(Exception ex, string errorMessage)
        {
            localLogger.LogError("Error occurred", ex);

            var validationMessages = new List<string>();            

            var result = Json(new PagedJsonResult
            {
                Errors = new ErrorJsonResult
                {
                    ValidationMessages = validationMessages.ToArray(),
                    Error = errorMessage,
                    DeveloperException = ex.ToString()
                }
            });

            result.StatusCode = (int)HttpStatusCode.BadRequest;
            return result;
        }
    }
}
