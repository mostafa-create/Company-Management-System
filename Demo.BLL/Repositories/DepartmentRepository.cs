using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
	public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
	{
        // i don't have to make dependancy injection because i do not have any functions here to require connecting to the database
        public DepartmentRepository(MVCAppDbContext dbContext):base(dbContext)
        {
            
        }
    }
}
