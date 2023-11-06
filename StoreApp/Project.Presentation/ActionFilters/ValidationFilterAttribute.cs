using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBAPIFramework.ActionFilters
{
    public class ValidationFilterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            // parametre bilgisi : parametre valuesinde dto var ise değerini al dedik
            var param = context.ActionArguments.SingleOrDefault(p=>p.Value.ToString().Contains("Dto")).Value;
            if (param is null)
            {
                context.Result = new BadRequestObjectResult(
                    $"Dto is null. \n" +
                    $"Controller : {controller} \n" +
                    $"Action : {action} \n");
                //400
                return; //uygulama sonlandırıldı
            }

            if(!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }
    }
}
