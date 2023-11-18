using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models.ViewModels
{
    public class CloseTaskViewModel
    {
        public int? Project_Id { get; set; }
        public int TaskId { get; set; }
        [Required(ErrorMessage = "Actual End date required")]
        [Display(Name = "ActualEndDate", ResourceType =typeof(TaskManagementSystem.Resources.CloseTask))]
        public string ActualEndDate { get; set; }

        public string TaskName { get; set; }
    }
}
