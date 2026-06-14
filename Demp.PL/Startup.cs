using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Demp.PL.MappingProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demp.PL
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
			services.AddControllersWithViews();
			services.AddDbContext<MVCAppDbContext>(Options =>
			{
				//to make the connection string more dynamic 
				Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});
			services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddAutoMapper(M => M.AddProfiles(new List<Profile>() { new EmployeeProfile(), new UserProfile(), new RoleProfile()}));// require a profile to map based on it -- it is like a protocol
			// here we have to write this cause createAsync use some services 
			services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
			{
				Options.Password.RequireNonAlphanumeric = true; // @ #
				Options.Password.RequireDigit = true;
				Options.Password.RequireLowercase = true;
				Options.Password.RequireUppercase = true;

			})
				.AddEntityFrameworkStores<MVCAppDbContext>()
				.AddDefaultTokenProviders();
			// allow dependancy injection for usermanager, signin manager, role manager
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)// generate cookies to monitore the user
																						 // whether he is authorized or not
				.AddCookie(Options =>
				{
					Options.LoginPath = "Account/Login";
					Options.AccessDeniedPath = "Home/Error";
				});


			#region diff between AddScoped vs AddSingleton vs AddTransient
			/*
             1. addsingleton
                use it if want to call the object at all requests
                Object LifeTime = Application LifeTime
				will be injected through the application lifetime
             2. addscoped
                use it if want to call the object but not at all requests
                Object LifeTime = Request LifeTime
				will be injected per request
			 3. addtransient
				use it if want to call the object per operation
				Object LifeTime = Operation LifeTime
				will be injected per operation so that does mean request may have more than one operation

             */
			#endregion
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Account}/{action=Login}/{id?}");
			});
		}
	}
}
