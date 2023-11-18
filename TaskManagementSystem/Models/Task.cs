using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Areas.Admin.Models;

namespace TaskManagementSystem.Models
{
    public class Task
    {
        [Key]
        
        public int Id { get; set; }
        //public int employee_id { get; set; }
        // 
        [Display(Name = "Employee")]
        [ForeignKey("AssignedTo")]
        public string Employee_AssignedTo { get; set; }

        //

        [ForeignKey("AssignedBy")]


        public string Employee_AssignedBy { get; set; }
        [Required(ErrorMessage = "Task name is required")]
        public string TaskName { get; set; }
        [ForeignKey("Project")]
        public int? Project_Id { get; set; }
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        public Employee AssignedTo { get; set; }


        public Employee AssignedBy { get; set; }

        public Project Project { get; set; }

        public ICollection<Comment> Comments;
        public string dummy { get; set; }

        public bool Closed { get; set; }

        [Display(Name = "Actual Start date")]
        public DateTime? ActualStartDate { get; set; }

        public DateTime? EndDate { get; set; }


       
        public DateTime? ActualEndDate { get; set; }












    }
}
