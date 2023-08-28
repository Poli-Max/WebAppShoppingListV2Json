using System;
using System.ComponentModel.DataAnnotations;

namespace Web_App_Shop_V2.Domain.ViewModels.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите имя или почту")]
    [MaxLength(30,ErrorMessage ="Введите не более 30 символов")]
    [MinLength(2, ErrorMessage = "Введите более 2 символов")]
    public string name { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [MinLength(3, ErrorMessage = "Введите более 3 символов")]
    public string password { get; set; }

    [Required(ErrorMessage = "Подтвердите")]
    [DataType(DataType.Password)]
    [Compare("password", ErrorMessage = "Несовпадают пароли")]
    public string passwordConfirm { get; set; }
}

