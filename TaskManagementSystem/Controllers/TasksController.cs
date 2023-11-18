using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using TaskManagementSystem.Areas.Admin.Models;
using TaskManagementSystem.Areas.Admin.ViewModel;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ViewModels;
using TaskManagementSystem.ServiceClasses;

namespace TaskManagementSystem.Areas.Admin.Controllers
{
    [ServiceFilter(typeof(CheckSessionActionFilter))]
    public class TasksController : Controller
    {
        private TaskManagementSystemDbContext _Context;
        private ITaskSerrvice _service;
       
        public TasksController(TaskManagementSystemDbContext Context, ITaskSerrvice service)
        {
            _Context = Context;
            _service = service;
          

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(int ProjectId)
        {

            byte[] UserIdSessionValue;
            IProjectsService ProjectService;
            ProjectService = new ProjectsServices();
            ViewBag.EmployeeList = new SelectList(_Context.Employee.ToList(), "UserName", "UserName");
           Project ProjectModel = ((ProjectsServices)ProjectService).GetProject(_Context, ProjectId);
            HttpContext.Session.TryGetValue("UserId", out UserIdSessionValue);


            TaskManagementSystem.Models.ViewModels.TaskViewModel TaskObject = new TaskManagementSystem.Models.ViewModels.TaskViewModel { Project_Id = ProjectId, Employee_AssignedBy = System.Text.Encoding.UTF8.GetString(UserIdSessionValue),StartDate=ProjectModel.StartingDate.ToString("dd/MM/yyyy"),EndDate=ProjectModel.EndingDate.ToString("dd/MM/yyyy") };
        

            return View(TaskObject);
        }
        [HttpPost]
        public IActionResult Create(TaskManagementSystem.Models.ViewModels.TaskViewModel TaskObject)
        {
            Project ProjectModel; ;
            IProjectsService ProjectService;
            ProjectService = new ProjectsServices();
            byte[] UserIdSessionValue;
            string StartingDate="", EndingDate="";
            
              
                StartingDate = Request.Form["StartingDate"];
                EndingDate = Request.Form["EndingDate"];
            try
            {
                ProjectModel = ((ProjectsServices)ProjectService).GetProject(_Context, (int)TaskObject.Project_Id);
                if (DateTime.ParseExact(EndingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(StartingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    ModelState.AddModelError("", "Task Start Date can't be greater Task End Date");

                }
                if (DateTime.ParseExact(StartingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) < ProjectModel.StartingDate)
                {
                    ModelState.AddModelError("", "Task Start Date can't be less than Project Start Date");
                }
                if (DateTime.ParseExact(EndingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) > ProjectModel.EndingDate)
                {
                    ModelState.AddModelError("", "Task End Date can't be greater Project End Date");
                }
                TaskObject.StartDate = StartingDate;
                TaskObject.EndDate = EndingDate;


                


            }


            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

            }
            if (ModelState.IsValid)
            {
                try
                {
                    ((TaskService)_service).CreateTask(_Context, TaskObject);
                    return RedirectToAction("ListByProject", new { ProjectId = TaskObject.Project_Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                }
            }

            ViewBag.EmployeeList = new SelectList(_Context.Employee.ToList(), "UserName", "UserName");
            TaskObject.StartDate = StartingDate;
            TaskObject.EndDate = EndingDate;

            return View(TaskObject);

        }
        public IActionResult ListByProject(int ProjectId)
        {
            ProjectsServices ProjectServiceObj;
            Project model;
            ProjectServiceObj = new ProjectsServices();
            byte[] SessionValue=null;
            string SessionValueAsString;
            model = ProjectServiceObj.GetProject(_Context, ProjectId);
            
            

            if (model != null)
            {
                ProjectViewModel ProjectViewModelObj = new ProjectViewModel
                {
                    Id = model.Id,
                    //ProjectName = model.ProjectName,
                    //StartingDate = model.StartingDate.ToString("dd/MM/yyyy"),
                    //EndingDate = model.EndingDate.ToString("dd/MM/yyyy"),
                    //ActualEndingDate = model.ActualEndingDate,
                    //Closed = model.Closed,
                    Tasks = model.Tasks.Count > 0 ? model.Tasks 
                            : new List<TaskManagementSystem.Models.Task>
                                    {
                                    new TaskManagementSystem.Models.Task { Project_Id = ProjectId }
                                 }
                    

                };
                ViewBag.ProjectName = model.ProjectName;
                //ProjectName = model.ProjectName,
                ViewBag.StartingDate = model.StartingDate.ToString("dd/MM/yyyy");
                ViewBag.EndingDate = model.EndingDate.ToString("dd/MM/yyyy");
                HttpContext.Session.TryGetValue("UserId", out SessionValue);
                SessionValueAsString = System.Text.Encoding.UTF8.GetString(SessionValue);
                ViewBag.CurrentUserId = SessionValueAsString;
                                
                HttpContext.Session.TryGetValue("isAdmin", out SessionValue);
                SessionValueAsString = System.Text.Encoding.UTF8.GetString(SessionValue);
                ViewBag.IsCurrentUserAdmin = SessionValueAsString == "True";
                return View("ListByProject", ProjectViewModelObj.Tasks);
            }
            return NotFound();



        }
        public IActionResult ListByEmployee()
        {
            List<TaskViewModel> TaskViewModelList= new List<TaskViewModel>();
            string  SessionValueAsString;
            byte[] SessionValue;
            try
            {
                HttpContext.Session.TryGetValue("UserId", out SessionValue);
                SessionValueAsString = System.Text.Encoding.UTF8.GetString(SessionValue);
                TaskViewModelList=((TaskService) _service).GetTaskByEmployee(_Context, SessionValueAsString);
                HttpContext.Session.TryGetValue("isAdmin", out SessionValue);
                SessionValueAsString = System.Text.Encoding.UTF8.GetString(SessionValue);
                ViewBag.IsCurrentUserAdmin = SessionValueAsString == "True";
               
            }
            catch(Exception ex)
            {

            }
            return View("TaskListByEmployee", TaskViewModelList);
        }
        public IActionResult ViewTask(int TaskId)
        {
            TaskService TaskServiceObj = new TaskService();
            string SessionValueAsString;
            byte[] SessionValue;
            TaskManagementSystem.Models.ViewModels.TaskViewModel TaskObject = TaskServiceObj.GetTask(_Context, TaskId);

            if(TempData["AddCommentExceptionMessage"]!=null)
            {
                ViewBag.AddCommentExceptionMessage = TempData["AddCommentExceptionMessage"];
            }
            HttpContext.Session.TryGetValue("isAdmin", out SessionValue);
            SessionValueAsString = System.Text.Encoding.UTF8.GetString(SessionValue);
            ViewBag.isAdmin = SessionValueAsString;
            HttpContext.Session.TryGetValue("UserId", out SessionValue);
            SessionValueAsString = System.Text.Encoding.UTF8.GetString(SessionValue);
            ViewBag.EmployeeList = new SelectList(_Context.Employee.Where(w=>w.UserName!=SessionValueAsString).ToList(), "UserName", "UserName");
           
            return View(TaskObject);
        }

        public IActionResult Delete(int Id,int ProjectId)
        {
            try
            {
         (   (TaskService)    _service).DeleteTask(_Context, Id);
            }
            catch (Exception ex)
            {
                TempData["TaskDeleteExceptionMessage"] = ex.Message;
            }
          
            return RedirectToAction("ListByProject", "Tasks",new { ProjectId = ProjectId });


        }

        public IActionResult CloseTask(int TaskId)
        {
            CloseTaskViewModel TVM = (from task in _Context.Tasks.Where(w => w.Id == TaskId)
                  
                   select new CloseTaskViewModel
                   {
                       TaskName = task.TaskName,
                       Project_Id = task.Project_Id,
                      
                       TaskId = task.Id


                   }).FirstOrDefault();

            return View(TVM);


        }
        [HttpPost]

        public IActionResult Close(CloseTaskViewModel tvm )
        {
            try
            {
                DateTime ActualCloseDate = DateTime.ParseExact(Request.Form["ActualEndDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ((TaskService)_service).CloseTask(_Context, tvm.TaskId, ActualCloseDate);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("ListByProject","Tasks", new { ProjectId = tvm.Project_Id });
            }
            return View("CloseTask", tvm);

        }


    }
}