using System;
using Web_App_Shop_V2.Domain.Enum;

namespace Web_App_Shop_V2.Domain.Models;

public class User
{
	public int id { get; set; }

	public string name { get; set; }

	public string password { get; set; }

	public TypeUser typeUser { get; set; }
}

