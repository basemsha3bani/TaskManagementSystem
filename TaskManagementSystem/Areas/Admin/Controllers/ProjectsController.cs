using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Models;
using TaskManagementSystem.ServiceClasses;
using TaskManagementSystem.Areas.Admin.Models;
using TaskManagementSystem.Areas.Admin.ViewModel;
using Microsoft.EntityFrameworkCore;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(CheckSessionActionFilter))]
    public class ProjectsController : Controller
    {
        // GET: /<controller>/
        private TaskManagementSystemDbContext _Context;
        private IProjectsService _service;

        public ProjectsController(TaskManagementSystemDbContext Context, IProjectsService service)
        {
            _Context = Context;
            _service = service;

        }
        public IActionResult Index()
        {
            ProjectsServices ProjectsObj;
            ProjectsObj = (ProjectsServices)_service;
            return View(((ProjectsServices) _service).ListProjects(_Context));
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(int Id)
        {
            Project model;
            model = ((ProjectsServices)_service).GetProject(_Context,Id);
            if(model!=null)
            {
                ProjectViewModel ViewModel = new ProjectViewModel
                {
                    Id = Id,
                    ProjectName = model.ProjectName,
                    StartingDate = model.StartingDate.ToString("dd/MM/yyyy"),
                    EndingDate = model.EndingDate.ToString("dd/MM/yyyy"),
                   
                   
                    Tasks = model.Tasks
                };
                return View("Edit", ViewModel);
            }
            return NotFound();

            
           
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                ((ProjectsServices)_service).DeleteProject(_Context, Id);
            }
            catch(Exception ex)
            {
                TempData["ProjectDeleteExceptionMessage"] = ex.Message;
            }
            return RedirectToAction("Index", "Admin/Projects");


        }
        [HttpPost]
        public IActionResult CreateProject(ProjectViewModel ProjectModel)
        {
            
            CultureInfo provider = CultureInfo.InvariantCulture;
            string StartingDate, EndingDate;
           
            StartingDate = Request.Form["StartingDate"];
            EndingDate = Request.Form["EndingDate"];
            if(DateTime.ParseExact(EndingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(StartingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
            {
                ModelState.AddModelError("", "Project ending date can not be less than project starting date");
            }
            if (ModelState.IsValid)
            {
                try
                {

                    ((ProjectsServices)_service).CreateProject(_Context, ProjectModel);
                    return RedirectToAction("Index", "Admin/Projects");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View("Create", ProjectModel);

                }


            }
            return View("Create",ProjectModel);
        }

        [HttpPost]
        public IActionResult EditProject(ProjectViewModel ProjectViewModel)
        {

            CultureInfo provider = CultureInfo.InvariantCulture;
            string StartingDate, EndingDate;


            StartingDate = Request.Form["StartingDate"];
            EndingDate = Request.Form["EndingDate"];
            if (DateTime.ParseExact(EndingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(StartingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
            {
                ModelState.AddModelError("", "Project ending date can not be less than project starting date");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    ((ProjectsServices)_service).EditProject(_Context, ProjectViewModel);
                      return RedirectToAction("Index", "Admin/Projects");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View("Edit", ProjectViewModel);

                }


            }
            return View("Edit", ProjectViewModel);
        }
        public IActionResult Tasks(int ProjectId)
        {
            Project model;
            model = ((ProjectsServices)_service).GetProject(_Context, ProjectId);
            if (model != null)
            {
                ProjectViewModel ViewModel = new ProjectViewModel
                {
                    Id = model.Id,
                    ProjectName = model.ProjectName,
                    StartingDate = model.StartingDate.ToString("dd/MM/yyyy"),
                    EndingDate = model.EndingDate.ToString("dd/MM/yyyy"),
                   
                    Closed = model.Closed,
                    Tasks = model.Tasks,
                     
                };
                return View("Tasks", ViewModel);
            }
            return NotFound();



        }

        public IActionResult CloseProject(int ProjectId)
        {
            CloseProjectViewModel CPVM = (from project in _Context.Projects.Where(w => w.Id == ProjectId).Include(inc=>inc.Tasks)

                                      select new CloseProjectViewModel
                                      {
                                          ProjectName = project.ProjectName,
                                          Id = project.Id,ActualEndingDate=(DateTime) project.Tasks.Select(s=>s.ActualEndDate).Max()
                                        


                                      }).FirstOrDefault();

            return View(CPVM);


        }
        [HttpPost]

        public IActionResult Close(CloseProjectViewModel cpvm)
        {
            try
            {
                DateTime ActualCloseDate = DateTime.ParseExact(Request.Form["ActualEndDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
             ( (ProjectsServices) _service).CloseProject(_Context, cpvm.Id, ActualCloseDate);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message); 

            }
            if (ModelState.IsValid)
            {
               return RedirectToAction("Index", "Admin/Projects");
               
            }
            return View("CloseProject", cpvm);

        }
    }
}












