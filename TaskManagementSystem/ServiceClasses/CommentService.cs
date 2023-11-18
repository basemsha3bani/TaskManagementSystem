using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.ServiceClasses
{
    public class CommentService : ICommentService
    {
        public void CreateComment(TaskManagementSystemDbContext _Context,CommentViewModel CommentVm)

        {
            Comment CommentModelObj = new Comment();
            CommentModelObj.CommentEmployeeId = CommentVm.CommentEmployeeId;
            CommentModelObj.CommentDate = CommentVm.CommentDate;
            CommentModelObj.Comments = CommentVm.Comments;
            CommentModelObj.CommentTaskId = CommentVm.CommentTaskId;
            CommentModelObj.CommentAttachment = CommentVm.CommentAttachment;

            _Context.Comments.Add(CommentModelObj);
            _Context.SaveChanges();
        }
        public Comment GetComment(TaskManagementSystemDbContext _Context,int CommentId)
        {
            return _Context.Comments.Where(w => w.Id == CommentId).FirstOrDefault();


        }
    }
}
