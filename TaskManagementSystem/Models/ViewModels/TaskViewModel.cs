using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ViewModels
{
    public class TaskViewModel
    {
       
         
            public int Id { get; set; }
            //public int employee_id { get; set; }
            // 
            [Display(Name = "Employee")]
           
            public string Employee_AssignedTo { get; set; }

            //

          


            public string Employee_AssignedBy { get; set; }
            [Required(ErrorMessage = "Task name is required")]
            public string TaskName { get; set; }
          
            public int? Project_Id { get; set; }

            public string ProjectName { get; set; }
            [Display(Name = "Start date")]
            public string StartDate { get; set; }

            public Employee AssignedTo { get; set; }


            public Employee AssignedBy { get; set; }

           

            public ICollection<CommentViewModel> Comments;
            public string dummy { get; set; }

            public bool Closed { get; set; }

            [Display(Name = "Actual Start date")]
            public string ActualStartDate { get; set; }

            public string EndDate { get; set; }











    }
    
}
