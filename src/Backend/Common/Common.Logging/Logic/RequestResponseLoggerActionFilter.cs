using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging.Logic.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Logging.Logic
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class RequestResponseLoggerActionFilter : Attribute, IActionFilter
    {
        private RequestResponseLogModel GetLogModel(HttpContext context)
        {
            return context.RequestServices.GetService<IRequestResponseLogModelCreator>().LogModel;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var model = GetLogModel(context.HttpContext);
            model.RequestDateTimeUtcActionLevel = DateTime.UtcNow;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var model = GetLogModel(context.HttpContext);
            model.ResponseDateTimeUtcActionLevel = DateTime.UtcNow;
        }
    }

    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class RequestResponseLoggerErrorFilter : Attribute, IExceptionFilter
    {
        private RequestResponseLogModel GetLogModel(HttpContext context)
        {
            return context.RequestServices.GetService<IRequestResponseLogModelCreator>().LogModel;
        }

        public void OnException(ExceptionContext context)
        {
            var model = GetLogModel(context.HttpContext);
            model.IsExceptionActionLevel = true;
            if (model.ResponseDateTimeUtcActionLevel == null)
            {
                model.ResponseDateTimeUtcActionLevel = DateTime.UtcNow;
            }
        }
    }
}
