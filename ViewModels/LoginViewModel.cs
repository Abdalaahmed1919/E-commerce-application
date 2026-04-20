using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Simple_E_commers_App.ViewModels
{
    public class LoginViewModel
    {
       [Required]
       [Remote(action: "IsEmailFound", controller: "Account")]
       [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
