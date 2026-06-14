using AutoMapper;
using Demo.DAL.Models;
using Demp.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demp.PL.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UserController(UserManager<ApplicationUser> userManager,
			IMapper mapper)
        {
			this.userManager = userManager;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				var Users = await userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FName = U.FName,
					LName = U.LName,
					Email = U.Email,
					PhoneNumber = U.PhoneNumber,
					Roles = userManager.GetRolesAsync(U).Result
				}).ToListAsync();
				return View(Users);
			}
			else
			{
				var User = await userManager.FindByNameAsync(SearchValue);
				var MappedUser = new UserViewModel()
				{
					Id = User.Id,
					FName = User.FName,
					LName = User.LName,
					Email = User.Email,
					PhoneNumber = User.PhoneNumber,
					Roles = userManager.GetRolesAsync(User).Result
				};
				return View(new List<UserViewModel>() { MappedUser});
			}
		}
		public async Task<IActionResult> Details(string Id, string ViewName = "Details")
		{
			if(Id is null)
				return BadRequest();
			var user = await userManager.FindByIdAsync(Id);
			if(user is null)
				return NotFound();
			var MappedUser = mapper.Map<ApplicationUser, UserViewModel>(user);
			return View(MappedUser);
		}
		public async Task<IActionResult> Edit(string Id)
		{
			return await Details(Id, "Edit");
		}
		[HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model, [FromRoute] string Id)
        {
            if(Id != model.Id)return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
					var User = await userManager.FindByIdAsync(Id);
					// the reason why i used manual mapping cause i want to edit user object that originally come from DB
					User.PhoneNumber = model.PhoneNumber;
					User.FName = model.FName;
					User.LName = model.LName;

                    await userManager.UpdateAsync(User);
					return RedirectToAction(nameof(Index));
				}
				catch(Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			return View(model);
        }
		public async Task<IActionResult> Delete(string Id)
		{
			return await Details(Id, "Delete");
		}
		[HttpPost]
		public async Task<IActionResult> ConfirmDelete(string Id)
		{
			try
			{
				var User = await userManager.FindByIdAsync(Id);
				await userManager.DeleteAsync(User);
				return RedirectToAction(nameof(Index));
			}
			catch(Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return RedirectToAction("Error", "Home");
			}
		}
    }
}
