using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Models;
using TaskManagementSystem.ServiceClasses;
//using LazZiya.ExpressLocalization;
using System.Globalization;
//using TaskManagementSystem.LocalizationResources;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Mvc.Razor;

namespace TaskManagementSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
//            var cultures = new[]
//{

//    new CultureInfo("ar"),

//    new CultureInfo("ar"),
//};
            services.AddDbContext<TaskManagementSystemDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(typeof(ILoginService), typeof(LoginService));
            services.AddScoped(typeof(IProjectsService), typeof(ProjectsServices));
            services.AddScoped(typeof(ITaskSerrvice), typeof(TaskService));
            services.AddScoped(typeof(ICommentService), typeof(CommentService));
            //services.AddScoped(typeof(IService), typeof(LoginService));
            //services.AddScoped(typeof(IService), typeof(ProjectsServices));
            //services.AddScoped(typeof(IService), typeof(TaskService));
            //services.AddScoped(typeof(IService), typeof(CommentService));
            services.AddScoped<CheckSessionActionFilter>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);//You can set Time   
            });
            //services.AddSingleton<IFileProvider>(
            //new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.AddLocalization();
            services.AddMvc()
                  //.AddViewLocalization(
                  //LanguageViewLocationExpanderFormat.Suffix,
                  //  opts => { opts.ResourcesPath = ""; }) options=>options.ResourcesPath= "Resources"
                  ;


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
             name: "areaRoute",
             template: "{area:exists}/{controller=Home}/{action=Index}");
            });
            app.UseRequestLocalization();
        }
    }
}
