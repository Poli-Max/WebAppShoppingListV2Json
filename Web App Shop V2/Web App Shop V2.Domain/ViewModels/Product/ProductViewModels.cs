using System;
using System.ComponentModel.DataAnnotations;

namespace Web_App_Shop_V2.Domain.ViewModels.Product;

public class ProductViewModels
{
    public int id { get; set; }

    [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
    public string name { get; set; }

    [Required(ErrorMessage = "Поле 'Описание' обязательно для заполнения")]
    [MaxLength(300, ErrorMessage = "Введите не более 300 символов")]
    public string description { get; set; }

    [Required(ErrorMessage = "Поле 'Стоимость' обязательно для заполнения")]
    public decimal price { get; set; }
}

