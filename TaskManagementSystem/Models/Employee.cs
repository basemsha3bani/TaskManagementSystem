using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Models
{
    public class Employee
    {

        // public string EmployeeId { get; set; }

        // public string Name { get; set; }

        public bool isAdmin { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        // public int UserId { get; set; }

        public ICollection<Task> TasksAssignedBy;

        public ICollection<Task> TasksAssignedTo;


    }
}