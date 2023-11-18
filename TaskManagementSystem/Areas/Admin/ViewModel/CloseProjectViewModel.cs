using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Areas.Admin.ViewModel
{
    public class CloseProjectViewModel
    {
        public int Id { get; set; }
       
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Project name is required")]

        [Display(Name = "Actual End Date"/*,ResourceType =typeof(ProjectResource)*/)]
        public DateTime ActualEndingDate { get; set; }


    }
}
