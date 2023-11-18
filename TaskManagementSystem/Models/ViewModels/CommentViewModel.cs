using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        
        public int CommentTaskId { get; set; }

        // public string CommentEmployeeName { get; set; }

        public string CommentEmployeeId { get; set; }

        public DateTime CommentDate { get; set; }
        [Required]
        public string Comments { get; set; }

        public string CommentAttachment { get; set; }

        

    }
}
