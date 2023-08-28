using System;
using Web_App_Shop_V2.DAL.Interfaces;
using Web_App_Shop_V2.DAL.Repositoties;
using Web_App_Shop_V2.Domain.Models;
using Web_App_Shop_V2.Service.Implementation;
using Web_App_Shop_V2.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Web_App_Shop_V2;

public static class Initializer
{
	public static void InitializeRepositories(this IServiceCollection service)
	{
		service.AddScoped<IBaseRepository<Product>, ProductRepository>();
        service.AddScoped<IBaseRepository<User>, UserRepository>();
    }

	public static void InitializeServices(this IServiceCollection service)
	{
		service.AddScoped<IProductService, ProductService>();
        service.AddScoped<IAccountService, AccountService>();
    }
}

