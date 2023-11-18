using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagementSystem.ServiceClasses
{
    public class CheckSessionActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            byte[] CultureValue;
            string CultureSessionKey, Language, Culture;
            string[] CultureValueDashSeperated;
            var routeValues = new RouteValueDictionary();

            if (context.HttpContext.Session.Keys.Where(w => w.ToString() == "UserId").FirstOrDefault() == null)
            {
                routeValues["controller"] = "Login";
                routeValues["action"] = "Index";
                context.Result = new RedirectToRouteResult(routeValues);
            }
            else
            {

                //CultureSessionKey = context.HttpContext.Session.Keys.Where(w => w.ToString() == "Culture").FirstOrDefault();
                //context.HttpContext.Session.TryGetValue(CultureSessionKey, out CultureValue);
                //string CultureValueAsString = System.Text.Encoding.UTF8.GetString(CultureValue);
                //CultureValueDashSeperated = CultureValueAsString.Split("-".ToCharArray()[0]);
                //Language = CultureValueDashSeperated[0];
                //Culture = CultureValueDashSeperated[1];
                ////Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", Language, Culture));
                ////Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", Language, Culture));
                //var cultureInfo = new CultureInfo(string.Format("{0}-{1}", Language, Culture));
               

                //CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                //CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
         
          
        }
    }
}
