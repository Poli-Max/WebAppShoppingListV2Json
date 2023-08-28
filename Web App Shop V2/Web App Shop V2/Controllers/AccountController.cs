using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Web_App_Shop_V2.Service.Implementation;
using Web_App_Shop_V2.Domain.Enum;
using Web_App_Shop_V2.Domain.ViewModels.Account;
using Web_App_Shop_V2.Service.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web_App_Shop_V2.Controllers;

public class AccountController : Controller
{
	private readonly IAccountService _accountService;

	public AccountController(IAccountService accountService)
	{
		this._accountService = accountService;
    }

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
			var response = await _accountService.Register(model);
			if(response.statusCode == Domain.Enum.StatusCode.OK)
			{
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(response.data));

				return RedirectToAction("Index", "Home");
			}
			ModelState.AddModelError("", response.description);
		}
		return View(model);
	}

	[HttpGet]
	public IActionResult Login()
	{
		return View();
	}

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
	{
		if (ModelState.IsValid)
		{
			var response = await _accountService.Login(model);
			if(response.statusCode == Domain.Enum.StatusCode.OK)
			{
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(response.data));

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", response.description);
        }
        return View(model);
    }

	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Logout()
	{
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }
}

