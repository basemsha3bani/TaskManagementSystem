using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Areas.Admin.Models;

namespace TaskManagementSystem.Areas.Admin.ViewModel
{
    public class ProjectReportDataViewModel
    {
       
       
        public string SearchExpression { get; set; }

      
      public List<ProjectViewModel> ProjectReportData { get; set; }


    }
}
