using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Task")]
        public int CommentTaskId { get; set; }

        // public string CommentEmployeeName { get; set; }

        public string CommentEmployeeId { get; set; }

        public DateTime CommentDate { get; set; }

        public string Comments { get; set; }

        public string CommentAttachment { get; set; }

        public Task Task { get; set; }


    }

}
