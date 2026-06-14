using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;

namespace Demp.PL.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
	{
        // another dependancy injection 
        private readonly IUnitOfWork unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
		{
            this.unitOfWork = unitOfWork;
        }
		public async Task<IActionResult> Index()
		{
            var departments = await unitOfWork.DepartmentRepository.GetAllAsync();
			return View(departments);
		}
		// work with httpget by default
		//httpget action
		public IActionResult Create()
		{
			return View();
		}
		// we cannot differenciate between actions by different parameters
		// so i had to differenciate it by which is work with post and get
		[HttpPost]
		public async Task<IActionResult> Create(Department department)
		{
			if (ModelState.IsValid)// to validate all the attriputes of department object
			{
				await unitOfWork.DepartmentRepository.AddAsync(department);// add to DB
				int NOA = await unitOfWork.Complete();
				//3. Temp Data => Dictionary Object
				// Transfer data from action to another action 
				if(NOA > 0)
				{
					TempData["Message"] = "Department Is Created";
				}
				return RedirectToAction("Index");// back to Index action after submit
			}
			return View(department);// if there is an error return with the same data
		}
		public async Task<IActionResult> Details(int? id, string ViewName = "Details")
		{
			if (id is null) return BadRequest();
			var department = await unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
			if (department is null) return NotFound();
			return View(ViewName, department);
		}
		public async Task<IActionResult> Edit(int? id)
		{
			//if (id is null) return BadRequest();
			//var department = unitOfWork.DepartmentRepository.GetById(id.Value);
			//if (department is null) return NotFound();
			//return View(department);
			return await Details(id, "Edit");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Department department, [FromRoute] int id)
		{
			if(id != department.Id) return BadRequest();
            if (ModelState.IsValid)// to validate all the attriputes of department object
            {
				try
				{
					unitOfWork.DepartmentRepository.Update(department);// Update into DB
					await unitOfWork.Complete();
					return RedirectToAction("Index");// back to Index action after submit
				}
				catch(System.Exception e)
				{ 
					ModelState.AddModelError(string.Empty, e.Message);
				}
            }
            return View(department);
        }
		public async Task<IActionResult> Delete(int? id)
		{
			//if (id is null) return BadRequest();
			//var department = unitOfWork.DepartmentRepository.GetById(id.Value);
			//if (department is null) return NotFound();
			//return View("Delete", department);
			return await Details(id, "Delete");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Department department, [FromRoute] int id)
		{
			if (id != department.Id) return BadRequest();
			if (ModelState.IsValid)// to validate all the attriputes of department object
			{
				try
				{
					unitOfWork.DepartmentRepository.Delete(department);// Update into DB
					await unitOfWork.Complete();
					return RedirectToAction("Index");// back to Index action after submit
				}
				catch (System.Exception e)
				{
					ModelState.AddModelError(string.Empty, e.Message);
				}
			}
			return View(department);
		}

	}
}
