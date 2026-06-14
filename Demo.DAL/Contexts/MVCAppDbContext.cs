using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
	public class MVCAppDbContext : IdentityDbContext<ApplicationUser>
	{
		// to connect to the DB
		public MVCAppDbContext(DbContextOptions<MVCAppDbContext> options) : base(options)
		{

		}
		// another way
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlServer("Server = .; Database = FirstDB; Trusted_Connection = True");
		//}

		public DbSet<Department> Departments { get; set; }
		public DbSet<Employee> Employees { get; set; }

	}
}
