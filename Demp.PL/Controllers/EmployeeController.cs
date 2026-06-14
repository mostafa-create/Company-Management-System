using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demp.PL.Helpers;
using Demp.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demp.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
	{
        // another dependancy injection 
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public EmployeeController(IUnitOfWork unitOfWork
			, IMapper mapper)
		{
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
		public async Task<IActionResult> Index(string SearchValue)
		{
			
            //1. Viewdata => KeyValuePair[Dictionary Object]
            //transfer data from contorller [action] to its view
            //.Net Framework 3.5
            //ViewData["Message"] = "Hello From View Data";

            //2. Viewbag => Dynamic Property [Based on Dynamic Keyboard]
            //transfer data from contorller [action] to its view
            //.Net Framework 4
            //ViewBag.Message = "Hello From View Bag";
			IEnumerable<Employee> employees;
			if (string.IsNullOrEmpty(SearchValue))
			{
				employees = await unitOfWork.EmployeeRepository.GetAllAsync();	
			}
			else
			{
				employees = unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);
			}
            var MappedEmployee = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployee);
		}
		// work with httpget by default
		//httpget action
		public async Task<IActionResult> Create()
		{
			ViewBag.Departments = await unitOfWork.DepartmentRepository.GetAllAsync();
			return View();
		}
		// we cannot differenciate between actions by different parameters
		// so i had to differenciate it by which is work with post and get
		[HttpPost]
		public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
		{
			if (ModelState.IsValid)// to validate all the attriputes of Employee object
			{
				// manual mapping
				//var MappedEmployee = new Employee()
				//{
				//	Name= employeeVM.Name,
				//	Age= employeeVM.Age,
				//	// and so on
				//}
				employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.image, "Images");
				var MappedEmployee = mapper.Map<EmployeeViewModel,  Employee>(employeeVM);

				await unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);// add to DB
				await unitOfWork.Complete();
				return RedirectToAction("Index");// back to Index action after submit
			}
			return View(employeeVM);// if there is an error return with the same data
		}
		public async Task<IActionResult> Details(int? id, string ViewName = "Details")
		{
			if (id is null) return BadRequest();
			var employee = await unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
			if (employee is null) return NotFound();
            var MappedEmployee = mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(ViewName, MappedEmployee);
		}
		public async Task<IActionResult> Edit(int? id)
		{
            //if (id is null) return BadRequest();
            //var Employee = unitOfWork.EmployeeRepository.GetById(id.Value);
            //if (Employee is null) return NotFound();
            //return View(Employee);
            ViewBag.Departments = await unitOfWork.DepartmentRepository.GetAllAsync();
            return await Details(id, "Edit");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
		{
			if (id != employeeVM.Id) return BadRequest();
			if (ModelState.IsValid)// to validate all the attriputes of Employee object
			{
				try
				{
					employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.image, "Images");
					var MappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
					unitOfWork.EmployeeRepository.Update(MappedEmployee);// Update into DB
                    await unitOfWork.Complete();
                    return RedirectToAction("Index");// back to Index action after submit
				}
				catch (System.Exception e)
				{
					ModelState.AddModelError(string.Empty, e.Message);
				}
			}

            return View(employeeVM);
		}
		public async Task<IActionResult> Delete(int? id)
		{
			//if (id is null) return BadRequest();
			//var Employee = unitOfWork.EmployeeRepository.GetById(id.Value);
			//if (Employee is null) return NotFound();
			//return View("Delete", Employee);
			return await Details(id, "Delete");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
		{
			if (id != employeeVM.Id) return BadRequest();
			if (ModelState.IsValid)// to validate all the attriputes of Employee object
			{
				try
				{
                    var MappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    unitOfWork.EmployeeRepository.Delete(MappedEmployee);// Update into DB
                    int AR = await unitOfWork.Complete();
					if(AR > 0 && employeeVM.ImageName is not null)
					{
						DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
					}
                    return RedirectToAction("Index");// back to Index action after submit
				}
				catch (System.Exception e)
				{
					ModelState.AddModelError(string.Empty, e.Message);
				}
			}
			return View(employeeVM);
		}

	}
}

