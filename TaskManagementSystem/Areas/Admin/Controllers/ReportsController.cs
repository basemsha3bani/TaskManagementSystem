using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Areas.Admin.ViewModel;
using TaskManagementSystem.Models;
using TaskManagementSystem.ServiceClasses;

namespace TaskManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportsController : Controller
    {
        private TaskManagementSystemDbContext _Context;
        private IProjectsService _service;

        public ReportsController(TaskManagementSystemDbContext Context, IProjectsService service)
        {
            _Context = Context;
            _service = service;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProjectReport()
        {
            return View(new ProjectReportDataViewModel { ProjectReportData= new List<ProjectViewModel>() });
        }
        [HttpPost]
        public IActionResult RetreiveProjectReportData(ProjectReportDataViewModel model)
        {
            ProjectReportDataViewModel resultmodel = new ProjectReportDataViewModel();
            resultmodel.SearchExpression = model.SearchExpression;
            resultmodel.ProjectReportData = (from projectrecs in
                                                 _Context.Projects.Where(w => w.ProjectName.Contains(model.SearchExpression) || model.SearchExpression == null)
                                                 select new ProjectViewModel
                                                 { 
                                                     Id=projectrecs.Id,
                                                     ProjectName=projectrecs.ProjectName,
                                                     Closed=projectrecs.Closed,
                                                     EndingDate=projectrecs.EndingDate.ToString("dd/MM/yyyy"),
                                                     StartingDate=projectrecs.StartingDate.ToString("dd/MM/yyyy"),
                                                     ActualEndingDate=projectrecs.ActualEndingDate.ToString("dd/MM/yyyy"),
                                                     Late=projectrecs.ActualEndingDate.Date> projectrecs.EndingDate.Date

                                                 }).ToList();
            return View("ProjectReport", resultmodel);
        }
    }
}