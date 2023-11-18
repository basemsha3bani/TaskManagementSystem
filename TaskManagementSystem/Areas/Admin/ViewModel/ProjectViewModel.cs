using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Areas.Admin.ViewModel
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Project name is required")]
       
        [Display(Name = "Name"/*,ResourceType =typeof(ProjectResource)*/)]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Project starting date is required")]
        [Display(Name = "StartingDate"/*, ResourceType = typeof(ProjectResource)*/)]
        public string StartingDate { get; set; }
        //[Display(Name = "EndingDate", ResourceType = typeof(ProjectResource))]
        [Required(ErrorMessage = "Project ending date is required")]
        [Display(Name = "EndingDate"/*, ResourceType = typeof(ProjectResource)*/)]
        public string EndingDate { get; set; }

        public string ActualEndingDate { get; set; }

        public bool Closed { get; set; }

        public ICollection<TaskManagementSystem.Models.Task> Tasks { get; set; }

        

        public bool IsCurrentUserAdmin { get; set; }

        public string CurrentUserId { get; set; }

        public bool Late { get; set; }
    }
}
