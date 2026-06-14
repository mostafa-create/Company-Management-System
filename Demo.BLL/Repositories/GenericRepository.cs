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
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		// here we use the Concept of Dependancy Injection in order to not open two different connections with the DB
		private readonly MVCAppDbContext _dbContext;
		// ask clr to inject the dbcontext object inside the constractor
		public GenericRepository(MVCAppDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task AddAsync(T item)
		{
			await _dbContext.AddAsync(item);
		}

		public void Delete(T item)
		{
			_dbContext.Remove(item);
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			if(typeof(T) == typeof(Employee))
			{
                return (IEnumerable<T>) await _dbContext.Employees.Include(E=>E.Department).ToListAsync();
            }
			return await _dbContext.Set<T>().ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			//there is 2 ways to get the department by iD and make sure that if i requested it before 
			// just get it don't make another request
			#region way1
			//var department = _dbContext.Departments.Local.Where(D => D.Id == id).FirstOrDefault();
			//if(department is null)
			//    department = _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();
			//return Department;
			#endregion
			#region way2
			return await _dbContext.Set<T>().FindAsync(id);
			#endregion
		}
		public void Update(T item)
		{
			_dbContext.Update(item);
		}
	}
}
