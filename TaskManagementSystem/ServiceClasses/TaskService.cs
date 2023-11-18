using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.ServiceClasses
{
    public class TaskService: ITaskSerrvice
    {
       public void CreateTask(TaskManagementSystemDbContext DbContextObj, Models.ViewModels.TaskViewModel TaskObject)
        {
            TaskManagementSystem.Models.Task TaskModelObj = new TaskManagementSystem.Models.Task();
            TaskModelObj.Employee_AssignedBy = TaskObject.Employee_AssignedBy;
            TaskModelObj.Employee_AssignedTo = TaskObject.Employee_AssignedTo;
            TaskModelObj.TaskName = TaskObject.TaskName;
            TaskModelObj.Project_Id = TaskObject.Project_Id;
            TaskModelObj.StartDate = DateTime.ParseExact(TaskObject.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //TaskManagementSystem.Models.Task task = DbContextObj.Tasks.AsNoTracking().Where(w => w.Project_Id == TaskObject.Project_Id).OrderByDescending(o => o.Id).FirstOrDefault();
            
            TaskModelObj.EndDate = DateTime.ParseExact(TaskObject.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //if (task != null)
            //{
            //    TaskModelObj.Id = int.Parse((task.Id + 1).ToString() + TaskObject.Project_Id.ToString());

            //}
            //else
            //{
            //    TaskModelObj.Id = int.Parse("1" + TaskObject.Project_Id.ToString());
            //}
            DbContextObj.Tasks.Add(TaskModelObj);
            DbContextObj.SaveChanges();

        }
        public void  DeleteTask(TaskManagementSystemDbContext DbContextObj, int TaskId)
        {
            DbContextObj.Tasks.RemoveRange(DbContextObj.Tasks.Where(w => w.Id == TaskId).ToList());
            DbContextObj.SaveChanges();
        }
        public List<TaskViewModel> GetTaskByEmployee(TaskManagementSystemDbContext DbContextObj,string EmployeeId)
        {
            List<TaskViewModel> TaskViewModelList= new List<TaskViewModel>();
            try
            {
                TaskViewModelList = (from recs in DbContextObj.Tasks.Where(w => w.Employee_AssignedTo == EmployeeId)
                                     select new TaskViewModel

                                     {
                                         Id=recs.Id,
                                         TaskName=recs.TaskName,
                                         StartDate=recs.StartDate.ToString("dd/MM/yyyy"),
                                         EndDate=((DateTime)recs.EndDate).ToString("dd/MM/yyyy")

                                         
                                         
                                     }).ToList();
            }
            catch(Exception ex)
            {

            }
            return TaskViewModelList;
            
        }

        public TaskViewModel GetTask(TaskManagementSystemDbContext DbContextObj,int TaskId)
        {

            TaskViewModel ReturnObject;
            ReturnObject = (from recs in DbContextObj.Tasks.Where(w => w.Id == TaskId)
                            select new TaskViewModel
                            {
                                Id = recs.Id,
                                TaskName = recs.TaskName,
                                StartDate = recs.StartDate.ToString("dd/MM/yyyy"),
                                EndDate = ((DateTime)recs.EndDate).ToString("dd/MM/yyyy"),
                                Comments = (from recs2 in DbContextObj.Comments
                                            where recs2.CommentTaskId == recs.Id

                                            join recs3 in DbContextObj.Employee
                                            on recs2.CommentEmployeeId equals recs3.UserName
                                            select new CommentViewModel
                                            {
                                                Comments = recs2.Comments,
                                                CommentDate = recs2.CommentDate,
                                                CommentEmployeeId = recs2.CommentEmployeeId,
                                                CommentAttachment = GetFileNameFromAttachment(recs2.CommentAttachment),
                                                Id = recs2.Id

                                            }).ToList(),
                                Employee_AssignedTo = ""

                                ,Project_Id=recs.Project_Id

                            }).FirstOrDefault();

            return ReturnObject;
        }

        private string GetFileNameFromAttachment(string AttachmmentPath)
        {
            string[] AttachmentParts= {""};
            if(AttachmmentPath==null)
            {
                return "";
            }
            try
            {
                AttachmentParts = AttachmmentPath.Split("\\".ToCharArray()[0]);
               return AttachmentParts[AttachmentParts.Length - 1];
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        public void ForwardTask(TaskManagementSystemDbContext DbContextObj,int TaskId,string EmployeeAssignedTo)
        {
            Models.Task task = DbContextObj.Tasks.Where(w => w.Id == TaskId).FirstOrDefault();
            if(task==null)
            {
                return;
            }
            task.Employee_AssignedTo = EmployeeAssignedTo;
            DbContextObj.Update(task);
            DbContextObj.SaveChanges();
        }
        public void CheckActualStartDate(TaskManagementSystemDbContext DbContextObj, int TaskId)
        {
            Models.Task task = DbContextObj.Tasks.Where(w => w.Id == TaskId).FirstOrDefault();
            if (task == null)
            {
                return;
            }
            if(DbContextObj.Comments.Where(w=>w.CommentTaskId==TaskId).Count()==0)
            {
                task.ActualStartDate = DateTime.Now;
            }

           ;
            DbContextObj.Update(task);
            DbContextObj.SaveChanges();
        }
        

        public void CloseTask(TaskManagementSystemDbContext DbContextObj, int TaskId, DateTime ActualEndDate)
        {
            Models.Task task = DbContextObj.Tasks.Where(w => w.Id == TaskId).FirstOrDefault();
            if (task == null)
            {
                return;
            }
            task.ActualEndDate = ActualEndDate;
            if (task.ActualEndDate.Value.Date<task.ActualStartDate.Value.Date)
            {
                throw new Exception("Actual end date can't be before actual start date");
            }
          

            task.Closed = true;
            DbContextObj.Update(task);
            DbContextObj.SaveChanges();
        }
    }
}
 