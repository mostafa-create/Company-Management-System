using Demo.DAL.Models;
using Demp.PL.Helpers;
using Demp.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demp.PL.Controllers
{
	[Authorize]
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
		#region Register
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var User = new ApplicationUser()
				{
					UserName = model.EmailAddress.Split("@")[0],
					Email = model.EmailAddress,
					IsAgree = model.IsAgree,
					FName = model.FName,
					LName = model.LName
				};
				var result = await userManager.CreateAsync(User, model.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("Login");
				}
				else
				{
					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(model);
		}
		#endregion
		#region Login
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var User = await userManager.FindByEmailAsync(model.EmailAddress);
				if (User is not null)
				{
					var result = await userManager.CheckPasswordAsync(User, model.Password);
					if (result)
					{
						var loginresult = await signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
						if(loginresult.Succeeded)
							return RedirectToAction("Index", "Home");
					}
					else
						ModelState.AddModelError(string.Empty, "Password Is Incorrect");
				}
				else
					ModelState.AddModelError(string.Empty, "Email Does Not Exists");
			}
			return View(model);
		}
		#endregion
		#region Sign Out
		public new async Task<IActionResult> SignOut()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
		#endregion
		#region ForgetPassword
		public IActionResult ForgetPassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var User = await userManager.FindByEmailAsync(model.EmailAddress);
				if(User is not null)
				{
					var token = await userManager.GeneratePasswordResetTokenAsync(User);
					var ResetPassordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, Token = token }, Request.Scheme);

					var email = new Email()
					{
						Subject = "Reset Password",
						To = model.EmailAddress,
						Body = ResetPassordLink
					};
					EmailSettings.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email Does Not Exists");
				}
			}
			return View("ForgetPassword",model);
		}
		public IActionResult CheckYourInbox()
		{
			return View();
		}
		#endregion

		#region ResetPassword
		public IActionResult ResetPassword(string Email, string Token)
		{
			TempData["Email"] = Email;
			TempData["Token"] = Token;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid) {
				string Email = TempData["Email"] as string;
				string Token = TempData["Token"] as string;
				var User = await userManager.FindByEmailAsync(Email);
				var result = await userManager.ResetPasswordAsync(User, Token, model.NewPassword);
				if(result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
				else
				{
					foreach(var error in result.Errors) {
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(model);
		}
		#endregion
	}
}
