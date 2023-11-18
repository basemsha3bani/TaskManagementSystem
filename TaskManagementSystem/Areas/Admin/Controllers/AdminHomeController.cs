 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.ServiceClasses;

namespace TaskManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(CheckSessionActionFilter))]
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}