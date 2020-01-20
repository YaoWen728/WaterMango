using System;
using System.Web.Mvc;

namespace TapMango.Domain.Validation
{
    public class WateringValidationAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                context.ExceptionHandled = true;
                context.Result = new JsonResult { Data = new { success = false, Message = context.Exception.Message } };
            }

        }
    }
}