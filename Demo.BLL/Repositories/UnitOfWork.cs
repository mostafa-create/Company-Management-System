using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
        private readonly MVCAppDbContext dbContext;

        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }

        public UnitOfWork(MVCAppDbContext dbContext) {
            EmployeeRepository = new EmployeeRepository(dbContext);
            DepartmentRepository = new DepartmentRepository(dbContext);
            this.dbContext = dbContext;
        }
        public async Task<int> Complete()
        {
            return await dbContext.SaveChangesAsync();
        }
        // closing the connection by the clr automatically 
        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
