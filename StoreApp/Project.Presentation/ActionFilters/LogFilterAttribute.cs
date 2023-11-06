using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Project.Entity.LogModel;
using Project.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBAPIFramework.ActionFilters
{
    public class LogFilterAttribute:ActionFilterAttribute
    {
        private readonly ILoggerService _logger;
        public LogFilterAttribute(ILoggerService logger)
        {
            _logger = logger;            
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.Log(Log("OnActionExecuting", context.RouteData), Project.Entity.Enums.LogType.Info);
        }
        private string Log(string ModelName, RouteData RouteData)
        {
            var logDetails = new LogDetails
            {
                Model = ModelName,
                Controller = RouteData.Values["controller"],
                Action = RouteData.Values["action"]
            };
            if (RouteData.Values["id"] is not null)
                logDetails.Id = RouteData.Values["id"];

            return logDetails.ToString();
        }
    }
}
