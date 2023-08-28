using System;
using System.Security.Claims;
using Web_App_Shop_V2.Domain.Models;
using Web_App_Shop_V2.Domain.Response;
using Web_App_Shop_V2.Domain.ViewModels.Account;

namespace Web_App_Shop_V2.Service.Interfaces;

public interface IAccountService
{
    Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);
    Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
}

