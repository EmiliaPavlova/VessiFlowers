namespace VessiFlowers.Business.Models.UserModels
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        [Display(Name = "Електонна поща")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        [EmailAddress(ErrorMessage = "Полето {0} не е валиден адрес на електронна поща")]
        public string Email { get; set; }

        [Display(Name = "Парола")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомни ме?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "Електонна поща")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        [EmailAddress(ErrorMessage = "Полето {0} не е валиден адрес на електронна поща")]
        public string Email { get; set; }

        [Display(Name = "Парола")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        [StringLength(100, ErrorMessage = "Полето {0} трябва да бъде с дължина поне {2} символа", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърдете паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Абонамент за бюлетин")]
        public bool SubscribeMe { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Display(Name = "Електонна поща")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        [EmailAddress(ErrorMessage = "Полето {0} не е валиден адрес на електронна поща")]
        public string Email { get; set; }

        [Display(Name = "Парола")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        [StringLength(100, ErrorMessage = "Полето {0} трябва да бъде с дължина поне {2} символа", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърдете паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Display(Name = "Електонна поща")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        [EmailAddress(ErrorMessage = "Полето {0} не е валиден адрес на електронна поща")]
        public string Email { get; set; }
    }
}
