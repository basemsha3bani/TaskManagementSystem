using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Areas.Admin.Resources;

namespace TaskManagementSystem.Areas.Admin.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
      
        public string ProjectName { get; set; }
       
        public DateTime StartingDate { get; set; }
       
     
        public DateTime EndingDate { get; set; }

        public bool Closed { get; set; }

        public List<TaskManagementSystem.Models.Task> Tasks { get; set; }

        public DateTime ActualEndingDate { get; set; }

    }
}
