using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ViewModels;
using TaskManagementSystem.ServiceClasses;

namespace TaskManagementSystem.Controllers
{
    [ServiceFilter(typeof(CheckSessionActionFilter))]
    public class CommentController : Controller
    {
        public CommentController(TaskManagementSystemDbContext Context, ICommentService service)
        {
            _Context = Context;
            _service = service;

        }
        private TaskManagementSystemDbContext _Context;
        private ICommentService _service;

        
        
        public IActionResult Index()
        {
            return View();
        }
       [HttpPost]
        public IActionResult AddComment(IFormFile file,CommentViewModel CommentVm)
        {
            _service = new CommentService();
            string SessionValueAsString;
            byte[] SessionValue;
            string TargetPath;
            TaskViewModel TVM=new TaskViewModel();

            try
            {
                HttpContext.Session.TryGetValue("UserId", out SessionValue);
                SessionValueAsString = System.Text.Encoding.UTF8.GetString(SessionValue);
                CommentVm.CommentEmployeeId = SessionValueAsString;
                CommentVm.CommentDate = DateTime.Now;
                
                TVM = (from task in _Context.Tasks.Where(w => w.Id == CommentVm.CommentTaskId)
                       join project in _Context.Projects
                       on task.Project_Id equals project.Id
                       select new TaskViewModel
                       {
                           TaskName = task.TaskName,
                           Project_Id = task.Project_Id,
                           ProjectName = project.ProjectName,Id=task.Id


                       }).FirstOrDefault();

                if (file != null)
                {
                    TargetPath = Path.Combine(
                  Directory.GetCurrentDirectory(), TVM.ProjectName, TVM.TaskName, SessionValueAsString);
                    if (!Directory.Exists(TargetPath))
                    {
                        Directory.CreateDirectory(TargetPath);



                    }

                    using (var stream = new FileStream(Path.Combine(TargetPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    CommentVm.CommentAttachment = Path.Combine(TargetPath, file.FileName);
                }
                    var t = _Context.Database.BeginTransaction();
                TaskService taskService = new TaskService();
                taskService.CheckActualStartDate(_Context, TVM.Id);
               ((CommentService) _service).CreateComment(_Context, CommentVm);
                
                if (Request.Form["hdnEmployee"] != "")
                    {
                        
                        taskService.ForwardTask(_Context, TVM.Id, Request.Form["hdnEmployee"]);
                        
                    }
                    t.Commit();
               
                


            }
            catch(Exception ex)
            {
                TempData["AddCommentExceptionMessage"]= ex.Message;
            }
            if (Request.Form["hdnEmployee"] != "")
            {

                HttpContext.Session.TryGetValue("isAdmin", out SessionValue);
                SessionValueAsString = System.Text.Encoding.UTF8.GetString(SessionValue);
                if(SessionValue.ToString()=="True")
                {
                    return RedirectToAction("ListByProject", "Tasks",new {ProjectId=TVM.Project_Id });
                }
                return RedirectToAction("ListByEmployee", "Tasks");
            }

            return RedirectToAction("ViewTask", "Tasks", new { TaskId = CommentVm.CommentTaskId });



        }

       
        public ActionResult ViewAttachment(int CommentId)
        {
            _service = new CommentService();
          Comment CommentObj= ((CommentService) _service).GetComment(_Context,CommentId);
            string filePath = CommentObj.CommentAttachment;
            
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/force-download", filePath);

        }
    }
}