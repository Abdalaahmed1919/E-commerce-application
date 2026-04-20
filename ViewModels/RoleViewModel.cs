using System.ComponentModel.DataAnnotations;

namespace Simple_E_commers_App.ViewModels
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Please Enter The Name ")]
        public string RoleName { get; set; }
    }
}
