using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class EmployeeHome : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}