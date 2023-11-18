using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using TaskManagementSystem.Areas.Admin.Models;
using System.Globalization;

namespace TaskManagementSystem.ServiceClasses
{
    public class ProjectsServices: IProjectsService
    {
      public  List<Areas.Admin.Models.Project> ListProjects(TaskManagementSystemDbContext DbContextObj)
        {
            List<Areas.Admin.Models.Project> ProjectsList;
            ProjectsList = (from project in DbContextObj.Projects
                            where !project.Closed
                            select new Areas.Admin.Models.Project
                            {
                                Id = project.Id,
                                ProjectName = project.ProjectName,
                                StartingDate = project.StartingDate,
                                EndingDate = project.EndingDate,
                                Tasks = (from t in DbContextObj.Tasks
                                         where t.Project_Id == project.Id
                                         select new Models.Task
                                         {
                                             Id = t.Id,
                                             TaskName = t.TaskName,
                                             Comments = (from comment in DbContextObj.Comments where comment.CommentTaskId == t.Id select comment).ToList()
                                            ,Closed=t.Closed
                                         }).ToList()
                            }).ToList();
                            
                            


            return ProjectsList;//DbContextObj.Projects.ToList();
        }
        public void CreateProject(TaskManagementSystemDbContext DbContextObj, TaskManagementSystem.Areas.Admin.ViewModel.ProjectViewModel ProjectObj)
        {
            Areas.Admin.Models.Project ProjectOb = new Areas.Admin.Models.Project();
            try
            {
               
                ProjectOb.ProjectName = ProjectObj.ProjectName;
                ProjectOb.StartingDate = DateTime.ParseExact(ProjectObj.StartingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ProjectOb.EndingDate = DateTime.ParseExact(ProjectObj.EndingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ProjectOb.Closed = ProjectObj.Closed;
                              
                DbContextObj.Projects.Add(ProjectOb);
                DbContextObj.SaveChanges();
               
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        public void EditProject(TaskManagementSystemDbContext DbContextObj, TaskManagementSystem.Areas.Admin.ViewModel.ProjectViewModel ProjectObj)
        {
            Areas.Admin.Models.Project ProjectOb = DbContextObj.Projects.Where(w => w.Id == ProjectObj.Id).FirstOrDefault();
            try
            {
                if(ProjectOb==null)
                {
                    return;
                }
               
                ProjectOb.ProjectName = ProjectObj.ProjectName;
                ProjectOb.StartingDate =DateTime.ParseExact(ProjectObj.StartingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ProjectOb.EndingDate = DateTime.ParseExact(ProjectObj.EndingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ProjectOb.Closed = ProjectObj.Closed;
             
                DbContextObj.Projects.Update(ProjectOb);
                DbContextObj.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Areas.Admin.Models.Project GetProject(TaskManagementSystemDbContext DbContextObj,int Id)
        {
            Areas.Admin.Models.Project ProjectOb = null;
           
            try
            {


                ProjectOb = 
                    (from project in DbContextObj.Projects.Where(w => w.Id == Id) select project)
                     .FirstOrDefault();
                if(ProjectOb!=null)
                {
                    ProjectOb.Tasks = (from task in DbContextObj.Tasks.Where(w => w.Project_Id == ProjectOb.Id&&!w.Closed)
                                       select new Models.Task
                                       {
                                           Project_Id = task.Project_Id,
                                           TaskName = task.TaskName,
                                           Id = task.Id,
                                           StartDate=task.StartDate,
                                           EndDate=task.EndDate,
                                           Employee_AssignedBy=task.Employee_AssignedBy,
                                           Employee_AssignedTo=task.Employee_AssignedTo,
                                           Comments = (from Comment in DbContextObj.Comments
                                                       where Comment.CommentTaskId == task.Id
                                                       select Comment).ToList()
                                       }).ToList();

                    
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return ProjectOb;
        }
        public void DeleteProject(TaskManagementSystemDbContext DbContextObj, int Id)
        {
            try
            {
                DbContextObj.Projects.Remove(DbContextObj.Projects.Where(w => w.Id == Id).FirstOrDefault());
                DbContextObj.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void CloseProject(TaskManagementSystemDbContext DbContextObj, int ProjectId, DateTime ActualEndDate)
        {
            Areas.Admin.Models.Project Project = DbContextObj.Projects.Where(w => w.Id == ProjectId).FirstOrDefault();
            if (Project == null)
            {
                return;
            }
            Project.ActualEndingDate = ActualEndDate;
            if (Project.ActualEndingDate.Date < Project.StartingDate.Date)
            {
                throw new Exception("Actual end date can't be before actual start date");
            }


            Project.Closed = true;
            DbContextObj.Update(Project);
            DbContextObj.SaveChanges();
        }

    }
}
