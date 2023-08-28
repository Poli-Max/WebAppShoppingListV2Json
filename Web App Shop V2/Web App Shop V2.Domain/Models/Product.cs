using System;
using System.ComponentModel.DataAnnotations;

namespace Web_App_Shop_V2.Domain.Models;

public class Product
{
    public int id { get; set; }

    public string name { get; set; }

    public string description { get; set; }

    public decimal price { get; set; }
}


