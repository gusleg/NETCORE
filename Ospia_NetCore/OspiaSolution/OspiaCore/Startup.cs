using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OspiaCore.Data;
using OspiaCore.Models;
using OspiaCore.Services;
using Microsoft.AspNetCore.Identity;

namespace OspiaCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) //estaba asi, le puse IServiceProvider serviceProvider para obtener los roles
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            //desahibilitamo la creacion de roles y  asignar roles usuario por hardcodeo
            //await CreateRoles(serviceProvider); //agrego para llamar el metodo que obttiene los roles
        }


        //creamos este metodo para asignar roles a los usuarios
        //ejemplo de crear roles y asiganr por harcodeo
        //private async Task CreateRoles(IServiceProvider serviceProvider)
        //{
        //    var roleMagar = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var userMagar = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    string[] rolesNames = { "Admin", "User" }; //aqui creo todos los roles necesarios por ejemplo {"Admin", "User","Contaduria", etc etc etc}
        //    IdentityResult result;
        //    foreach (var rolesName in rolesNames)
        //    {
        //        var roleExist = await roleMagar.RoleExistsAsync(rolesName);
        //        if (!roleExist) //si no existe
        //        {
        //            result = await roleMagar.CreateAsync(new IdentityRole(rolesName));
        //        }
        //    }
        //    // le asigno hardcor al usuario con id 1154a5b8-98c3-43fe-8e2d-698aa0855a51, el role Admin
        //    //var user = await userMagar.FindByIdAsync("1154a5b8-98c3-43fe-8e2d-698aa0855a51");
        //    //await userMagar.AddToRoleAsync(user, "Admin");
        //}
    }
}
