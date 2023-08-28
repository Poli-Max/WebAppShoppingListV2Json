using System;
using System.ComponentModel.DataAnnotations;

namespace Web_App_Shop_V2.Domain.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите имя или почту")]
    [MaxLength(30, ErrorMessage = "Введите не более 30 символов")]
    [MinLength(2, ErrorMessage = "Введите более 2 символов")]
    public string name { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string password { get; set; }
}

