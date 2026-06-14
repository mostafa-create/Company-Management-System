using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
	public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
	{
		// here i have to use dependancy injection because i want to have a connection with DB to implemnt the function below it
		private readonly MVCAppDbContext dbContext;

		public EmployeeRepository(MVCAppDbContext dbContext):base(dbContext)
        {
			this.dbContext = dbContext;
		}
		public IQueryable<Employee> GetEmployeesByAddress(string address)
		{
			return dbContext.Employees.Where(E => E.Address == address);
		}

        public IQueryable<Employee> GetEmployeesByName(string SearchValue)
        {
			return dbContext.Employees.Where(E => E.Name.ToLower().Contains(SearchValue.ToLower())).Include(E => E.Department);
        }
    }
}


