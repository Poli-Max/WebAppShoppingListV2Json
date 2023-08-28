using System;
using System.Security.Claims;
using Web_App_Shop_V2.DAL.Interfaces;
using Web_App_Shop_V2.Domain.Response;
using Web_App_Shop_V2.Domain.ViewModels.Account;
using Web_App_Shop_V2.Domain.Enum;
using Web_App_Shop_V2.Domain.Models;
using Web_App_Shop_V2.Domain.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Web_App_Shop_V2.Service.Interfaces;

namespace Web_App_Shop_V2.Service.Implementation;

public class AccountService : IAccountService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly ILogger<AccountService> _logger;

    public AccountService(IBaseRepository<User> userRepository, ILogger<AccountService> logger)
    {
        this._userRepository = userRepository;
        this._logger = logger;
    }

    public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
    {
        try
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.name == model.name);
            if (user != null)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    description = "Такой пользователь уже есть"
                };
            }

            user = new User()
            {
                name = model.name,
                password = EncryptionPassword.HashPassword(model.password),
                typeUser = TypeUser.defaultUser,
            };

            await _userRepository.Create(user);
            var result = Authentificate(user);

            return new BaseResponse<ClaimsIdentity>()
            {
                data = result,
                description = "Oбъект успешно добавился",
                statusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[Register]: {ex.Message}");
            return new BaseResponse<ClaimsIdentity>()
            {
                description = ex.Message,
                statusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
    {
        try
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.name == model.name);
            if (user == null)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    description = "Пользователь не найден"
                };
            }

            if (user.password != EncryptionPassword.HashPassword(model.password))
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    description = "Неверный пароль"
                };
            }

            var result = Authentificate(user);

            return new BaseResponse<ClaimsIdentity>()
            {
                data = result,
                statusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[Login]: {ex.Message}");
            return new BaseResponse<ClaimsIdentity>()
            {
                description = ex.Message,
                statusCode = StatusCode.InternalServerError
            };
        }
    }

    private ClaimsIdentity Authentificate(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.name),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.typeUser.ToString())
        };
        return new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    }
}

