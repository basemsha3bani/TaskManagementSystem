using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.ServiceClasses
{
    public class LoginService : ILoginService
    {
        public Employee emp { get; set; }
        
        public bool CheckLogin(TaskManagementSystemDbContext _Context,EmployeeLogin LoginModel)
        {
            emp= _Context.Employee.Where(w => w.UserName == LoginModel.UserName && w.Password == LoginModel.Password).FirstOrDefault();
            return (emp != null);
                

        }

    }
}
