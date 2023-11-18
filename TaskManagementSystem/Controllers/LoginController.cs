using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ViewModels;
using TaskManagementSystem.ServiceClasses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        // GET: /<controller>/
        private TaskManagementSystemDbContext _Context;
        private ILoginService _service;

        public LoginController(TaskManagementSystemDbContext Context, ILoginService service)
        {
            _Context = Context;
            _service = service;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPost(EmployeeLogin EmployeeLoginModel)
        {
            LoginService LoginObj;
            LoginObj = (LoginService)_service;
            try
            {
                if (LoginObj.CheckLogin(_Context, EmployeeLoginModel))
                {
                    HttpContext.Session.SetString("UserId", LoginObj.emp.UserName);
                    HttpContext.Session.SetString("isAdmin", LoginObj.emp.isAdmin.ToString());
                    //HttpContext.Session.SetString("Culture", Request.Form["hdnLang"]);
                    SpecifyCulture();
                    if (LoginObj.emp.isAdmin)
                    {
                        return RedirectToAction("Index", "Admin/AdminHome");
                    }
                    else
                    {

                        return RedirectToAction("ListByEmployee", "Tasks");
                    }
                }
                ModelState.AddModelError("", "InvalidLogin");
                return View("Index", EmployeeLoginModel);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", EmployeeLoginModel);
            }

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index");
        }
        //private void SpecifyCulture()
        //{
        //   // var cultureInfo = new CultureInfo(Request.Form["hdnLang"]);
        //    string lang = Request.Form["hdnLang"];
        //    //hdnLang is a hidden field in the login form 
        //    //filled by a default value 
        //    //and this value gets changed when  selecting an option from culture drop down
        //    System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
        //    System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);
           



        //}

        private void SpecifyCulture()
        {
            
            string lang = Request.Form["hdnLang"];
            var cultureInfo = new CultureInfo(Request.Form["hdnLang"]);
            //hdnLang is a hidden field in the login form 
            //filled by a default value 
            //and this value gets changed when  selecting an option from culture drop down
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;




        }

    }
}